﻿using System;
using System.Threading.Tasks;
using SteamWeb;
using System.Threading;
using System.Diagnostics;
using System.IO;
using ArkServer.Logging;
using System.Runtime.Serialization.Formatters.Binary;
using ArkServer.ServerMods;
using System.ComponentModel;
using System.Timers;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using SteamWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steam;
using Rcon;
using ArkServer.ServerUtilities;
using DiscordWebhook;

namespace ArkServer
{
    [Serializable()]
    public class Server : INotifyPropertyChanged
    {
        [NonSerialized()] private static readonly string SaveFolderName = "Server";
        [NonSerialized()] private static readonly string SaveDataFormat = ".dat";
        [NonSerialized()] public Process ArkProcess = null;
        [NonSerialized()] private readonly object _lock = new object();


        string _ServerName;
        ServerState _serverState;
        string _InstalledVersion;

        public string ServerName
        {
            get { return _ServerName; }
            set
            {
                _ServerName = value;
                OnPropertyChanged("ServerName");
            }
        }

        public ServerState serverState
        {
            get { return _serverState; }
            set
            {
                lock (_lock)
                {
                    _serverState = value;
                    OnPropertyChanged("serverState");
                }
            }
        }

        public string InstalledVersion
        {
            get { return _InstalledVersion; }
            set
            {
                _InstalledVersion = value;
                OnPropertyChanged("InstalledVersion");
            }
        }



        public string ArkSurvivalFolder { get; set; } = "C:\\SERVER\\TheIsland";
        public string Map { get; set; } = "TheIsland";
        public string ServerIp { get; set; } = "127.0.0.1";
        public string ServerStartArgument { get; set; } = "TheIsland?listen?SessionName=<server_name>?ServerPassword=<join_password>?ServerAdminPassword=<admin_password> -server -log ";
        public int QueryPort { get; set; } = 27015;
        public int Port { get; set; } = 7777;
        public int RconPort { get; set; } = 27020;
        public bool RconEnabled { get; set; } = true;
        public int maxPlayer { get; set; } = 60;
        public string RconPassword { get; set; } = "password";


        [NonSerialized()] public ModCollection Mods = null;
        [NonSerialized()] public ServerLog logs = null;
        [NonSerialized()] TimeSpan TotalProcessTime = TimeSpan.Zero;
        [NonSerialized()] int numOfTimeOuts = 0;
        [NonSerialized()] int DateofUpdate = 0;

        public Server(FileInfo file)
        {
            var SavedData = LoadFromFile(file);

            if (null != SavedData)
            {
                ServerName = SavedData.ServerName;
                ArkSurvivalFolder = SavedData.ArkSurvivalFolder;
                Map = SavedData.Map;
                ServerIp = SavedData.ServerIp;
                ServerStartArgument = SavedData.ServerStartArgument;
                QueryPort = SavedData.QueryPort;
                Port = SavedData.Port;
                RconPort = SavedData.RconPort;
                RconEnabled = SavedData.RconEnabled;
                maxPlayer = SavedData.maxPlayer;
                RconPassword = SavedData.RconPassword;
                CheckInstalledVersion();
                serverState = ServerState.Stopped;
                logs = new ServerLog(ServerName);
                Mods = new ModCollection(logs);

                if (!ServerCollection.MServerCollection.AddServer(this))
                {
                    this.Delete();
                }
            }
        }

        public Server(string key)
        {
            ServerName = key;

            if (ServerCollection.MServerCollection.AddServer(this))
            {
                SaveToFile(); /* save in file  */
                this.Delete(); /* server is now saved in the server collection -> delete this temp server */
            }

            logs = new ServerLog(ServerName);
            CheckInstalledVersion();
        }

        public void Delete()
        {
        }

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        internal void CheckInstalledVersion()
        {
            string path = Path.Combine(ArkSurvivalFolder, "version.txt");

            InstalledVersion = "unknown";
            if (File.Exists(path))
            {
                StreamReader file = new StreamReader(path, true);
                string version = file.ReadLine();

                if (!string.IsNullOrWhiteSpace(version))
                {
                    InstalledVersion = version;
                }
                file.Close();

            }
        }

        public void DeliteSaveFile()
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, ServerName + SaveDataFormat)))
            {
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, ServerName + SaveDataFormat));
            }
        }


        public void SaveToFile()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName)))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName));
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (ServerName + SaveDataFormat)), FileMode.Create, FileAccess.Write);
            formatter.Serialize(writerFileStream, this);
            writerFileStream.Close();
        }

        private Server LoadFromFile(FileInfo file)
        {
            Server server = null;

            if (true == File.Exists(file.FullName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream readerFileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                if (readerFileStream.Length > 0)
                {
                    server = (Server)formatter.Deserialize(readerFileStream);
                }
                readerFileStream.Close();
            }
            return server;
        }

        public async Task<AppInfo> DetermieAppInfo()
        {
            SteamCMDCall steamCMDCall = new SteamCMDCall();
            return await steamCMDCall.AppInfo();
        }

        public async Task InitServer()
        {
            logs.AddLog(LogType.Information, "Initialize server");
            serverState = ServerState.Initialize;
            await Mods.DetermineMods(ArkSurvivalFolder);
            AppInfo appinfo = new AppInfo();
            appinfo = await DetermieAppInfo();

            /*3218646 is public brunch for ark server  */
            if ((appinfo.buildid == 3218646) && (0 != appinfo.timeupdated))
            {
                DateofUpdate = appinfo.timeupdated;
            }
            else
            {
                logs.AddLog(LogType.Critical, "Can not determine current server update time. Updater does not work until next update time check (5min)");
            }


        }

        public void WatchServer(object sender, EventArgs e)
        {
            var newtotalTime = ArkProcess.TotalProcessorTime;

            if (TotalProcessTime == newtotalTime)
            {
                logs.AddLog(LogType.Developer, "Server timeout");
                numOfTimeOuts++;
            }


            if (numOfTimeOuts > 5)
            {
                logs.AddLog(LogType.Critical, "Process killed ");
                ArkProcess.Kill();
            }

            TotalProcessTime = newtotalTime;

        }

        public async void CheckforUpdatesAsync(object sender, EventArgs e)
        {
            AppInfo appinfo = new AppInfo();
            appinfo = await DetermieAppInfo();
            bool needsServerUpdate = false;
            bool needsModUpdate = false;

            /*3218646 is public brunch for ark server  */
            if ((appinfo.buildid == 3218646) && (0 != appinfo.timeupdated))
            {
                if ((DateofUpdate != 0) && (DateofUpdate < appinfo.timeupdated))
                {
                    needsServerUpdate = true;
                    logs.AddLog(LogType.Information, "Ark server update is available");
                }
                else
                {
                    logs.AddLog(LogType.Information, "Ark server is up to date");
                }

                if ((DateofUpdate != 0))
                {
                    DateofUpdate = appinfo.timeupdated;
                }
            }

            if ((DateofUpdate == 0) && (DateofUpdate == 0))
            {
                logs.AddLog(LogType.Critical, "Can not determine current server update time. Updater does not work until next update time check (5min)");
            }

            if (await Mods.CheckModsForUpdates())
            {
                needsModUpdate = true;
            }


            if (true == needsServerUpdate)
            {
                Utilities util = new Utilities(this);
                await util.ServerRestart(30, "ark update");

                needsModUpdate = false;
            }

            if (true == needsModUpdate)
            {
                Utilities util = new Utilities(this);
                await util.ServerRestart(30, "mod update");

            }


        }

        public async void RconDebugAsync()
        {
            Utilities util = new Utilities(this);
            await util.ServerRestart(0, "test" );
        }




        public void StartServer()
        {
            ArkProcess = new Process();
            ArkProcess.StartInfo.FileName = Path.Combine(ArkSurvivalFolder  ,"ShooterGame" , "Binaries" , "Win64" , "ShooterGameServer.exe");
            ArkProcess.StartInfo.Arguments = ServerStartArgument + " ";
            //ArkProcess.StartInfo.UseShellExecute = true;
            //ArkProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;


            logs.AddLog(LogType.Information, "Starting server");
            serverState = ServerState.Running;


            System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(20).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(WatchServer);

            ArkProcess.Start();
            TotalProcessTime = TimeSpan.Zero;
            numOfTimeOuts = 0;
            timer.Start();
            ArkProcess.WaitForExit();
            timer.Stop();
            ArkProcess.Close();


            if (serverState != ServerState.Stopped)
            {
                serverState = ServerState.Crashed;
                logs.AddLog(LogType.Critical, "Server crashed");
            }
        }


        public void UpdateServer()
        {

            SteamCMDCall steamCMDCall = new SteamCMDCall();
            logs.AddLog(LogType.Information, "Updating server");
            
            serverState = ServerState.Updating;

            var output = steamCMDCall.UpdateServer(this.ArkSurvivalFolder);
            output.Wait();


            logs.AddLog(LogType.Information, "Server update finished");
            CheckInstalledVersion();
        }
        public void StartServerHandler()
        {
            Webhook webhook = new Webhook("https://discordapp.com/api/webhooks/506598531579248660/QF5eqSWaTVD98q1rbIPqKZ0yyF1cAtIVmC5UzOxcwUN8VoAtEJaqunwdyHi8OMToyiGn");
            new Thread(async () =>
           {
               Thread.CurrentThread.IsBackground = true;
               do
               {
                   System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
                   timer.AutoReset = true;
                   timer.Elapsed += new ElapsedEventHandler(CheckforUpdatesAsync);

                   await InitServer();
                   UpdateServer();
                   timer.Start();
                   await webhook.Send(ServerName +"server is booting");
                   StartServer();
                   timer.Stop();


               }
               while (serverState != ServerState.Stopped);


           }).Start();
        }
    }
}
