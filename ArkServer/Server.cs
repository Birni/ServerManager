using System;
using System.Threading.Tasks;
using Steam;
using System.Threading;
using System.Diagnostics;
using System.IO;
using ArkServer.Logging;
using System.Runtime.Serialization.Formatters.Binary;

using System.ComponentModel;


namespace ArkServer
{
   [Serializable()]
   public class Server : INotifyPropertyChanged
    {
        [NonSerialized()] private static readonly string SaveFolderName = "Server";
        [NonSerialized()] private static readonly string SaveDataFormat = ".dat";

        string _ServerName;
        ServerState _serverState;
        string _InstalledVersion;

        public string ServerName
        {
            get { return _ServerName;  }
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
                _serverState = value;
                OnPropertyChanged("serverState");
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



        public string ArkSurvivalFolder { get; set; }
        public string Map { get; set; }
        public string ServerIp { get; set; }
        public string ServerStartArgument { get; set; }
        public int QueryPort { get; set; }
        public int Port { get; set; }
        public int RconPort { get; set; }
        public bool RconEnabled { get; set; }
        public int maxPlayer { get; set; }
        


        [NonSerialized()] public ServerLog logs = null; 

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
                CheckInstalledVersion();
                serverState = ServerState.Stopped;
                logs = new ServerLog(ServerName);

                if (!ServerCollection.MServerCollection.AddServer(this))
                {
                    this.Delete();
                }

                logs = new ServerLog(ServerName);

            }


        }
        public Server(string key)
        {
            ServerName = key;
            ArkSurvivalFolder = "C:\\SERVER\\TheIsland";
            Map = "TheIsland";
            ServerIp = "127.0.0.1";
            ServerStartArgument = "TheIsland?listen?SessionName=<server_name>?ServerPassword=<join_password>?ServerAdminPassword=<admin_password> -server -log ";
            QueryPort = 27015;
            Port = 7777;
            RconPort = 27020;
            RconEnabled = true;
            maxPlayer = 60;
            

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
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , SaveFolderName , ServerName + SaveDataFormat)))
            {
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, ServerName + SaveDataFormat));
            }
        }


        public void SaveToFile()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , SaveFolderName)))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , SaveFolderName));
            }

            string filename = ServerName + SaveDataFormat;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , SaveFolderName , (ServerName + SaveDataFormat)) , FileMode.Create, FileAccess.Write);
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


        public void InitServer()
        {
            logs = new ServerLog(ServerName);
        }

        public async Task<bool> StartServer()
        {
            if (Directory.Exists(ArkSurvivalFolder))
            {
                return await Task.Run(() =>
                {
                    Process pProcess = new Process();
                    pProcess.StartInfo.FileName = Path.Combine(ArkSurvivalFolder + "\\ShooterGame\\Binaries\\Win64\\ShooterGameServer.exe");
                    pProcess.StartInfo.Arguments = ServerStartArgument +" ";

                    if (!(logs == null))
                    {
                        logs.AddLog( LogType.Information,  "Starting server"); 
                    }
                    serverState = ServerState.Running;

                    pProcess.Start();
                    pProcess.WaitForExit();
                    pProcess.Close();

                    serverState = ServerState.Crashed;
                    logs.AddLog(LogType.Critical, "Server crashed");

                    return true;
                });
            }
            return false;
        }

        public  void StartAndUpdateServer()
        {
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                SteamCMDCall steamCMDCall = new SteamCMDCall();

                if (!(logs == null))
                {
                    logs.AddLog(LogType.Information, "Updating server");
                }
                serverState = ServerState.Updating;

                var output = steamCMDCall.UpdateServer(this.ArkSurvivalFolder);
                output.Wait();

                if (!(logs == null))
                {
                    logs.AddLog(LogType.Information, "Server update finished");
                }

                CheckInstalledVersion();
                await StartServer();
                

            }).Start();
        }

    }
}
