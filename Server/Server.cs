using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Steam;
using System.Threading;
using System.Diagnostics;
using System.IO;
using ArkServer.Logging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace ArkServer
{
   [Serializable()]
   public class Server 
    {
        [NonSerialized()] private static readonly string SaveFolderName = "Server";
        [NonSerialized()] private static readonly string SaveDataFormat = ".dat";

        public string ServerName { get; set; }
        public string ArkSurvivalFolder { get; set; }
        public string Map { get; set; }
        public string ServerIp { get; set; }
        public string ServerStartArgument { get; set; }
        public int QueryPort { get; set; }
        public int Port { get; set; }
        public int RconPort { get; set; }
        public bool RconEnabled { get; set; }
        public int maxPlayer { get; set; }


        [NonSerialized()] private ServerLog logs = null;

        public Server(FileInfo file)
        {
            var SavedData = LoadFromFile(file);

            if (null != SavedData)
            {
                ServerName = SavedData.ServerName;
                ArkSurvivalFolder = SavedData.ArkSurvivalFolder;
                Map = SavedData.Map;
                ServerIp = SavedData.ServerIp;
                ServerStartArgument = SavedData.ServerName;
                QueryPort = SavedData.QueryPort;
                Port = SavedData.Port;
                RconPort = SavedData.RconPort;
                RconEnabled = SavedData.RconEnabled;
                maxPlayer = SavedData.maxPlayer;

                if (!ServerCollection.MServerCollection.AddServer(this))
                {
                    this.Delete();
                }

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
                this.Delete();
            }
            else
            {
                SaveToFile();
            }
        }

        public void Delete()
        {
        }

        public void DeliteSaveFile()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + SaveFolderName + "\\" + ServerName + SaveDataFormat))
            {
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + SaveFolderName + "\\" + ServerName + SaveDataFormat);
            }
        }


        private void SaveToFile()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\"+ SaveFolderName))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + SaveFolderName);
            }

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream writerFileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\"+ SaveFolderName +"\\"+ServerName + SaveDataFormat , FileMode.Create, FileAccess.Write);
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
                server = (Server) formatter.Deserialize(readerFileStream);
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
            if (Directory.Exists(this.ArkSurvivalFolder))
            {
                return await Task.Run(() =>
                {
                    Process pProcess = new Process();
                    pProcess.StartInfo.FileName = this.ArkSurvivalFolder + "\\ShooterGame\\Binaries\\Win64\\ShooterGameServer.exe";
                    pProcess.StartInfo.Arguments = this.ServerStartArgument +" ";

                    if (!(logs == null))
                    {
                        logs.LogServerInformation("Staring server");
                    }

                    pProcess.Start();
                    pProcess.WaitForExit();
                    pProcess.Close();

                    if (!(logs == null))
                    {
                        logs.LogServerInformation("Server stopped");
                    }

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
                    logs.LogServerInformation("Updating server");
                }

                var output = steamCMDCall.UpdateServer(this.ArkSurvivalFolder);
                output.Wait();

                if (!(logs == null))
                {
                    logs.LogServerInformation("Server update finished");
                }

                await StartServer();

            }).Start();
        }

    }
}
