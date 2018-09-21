using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
   public class IServer
    {
        int ServerIdentifier { get; set; }
        string ServerName { get; set; }
        string ArkSurvivalFolder { get; set; }
        string Map { get; set; }
        string ServerIp { get; set; }
        string ServerStartArgument { get; set; }
        int QueryPort { get; set; }
        int Port { get; set; }
        bool RconEnabled { get; set; }
        int RconPort { get; set; }
        int maxPlayer { get; set; }


        public IServer(string name)
        {
            ServerName = name;
            ArkSurvivalFolder = "C:\\SERVER\\TheIsland";
            Map = "TheIsland";
            ServerIp = "127.0.0.1";
            ServerStartArgument = "TheIsland?listen?SessionName=<server_name>?ServerPassword=<join_password>?ServerAdminPassword=<admin_password> -server -log ";
            QueryPort = 27015;
            Port = 7777;
            RconPort = 27020;
            RconEnabled = true;
            maxPlayer = 60;
        }

        public string GetServerName()
        {
            return ServerName;
        }

        public string GetServerArkSurvivalFolder()
        {
            return ArkSurvivalFolder;
        }

        public string GetServerMap()
        {
            return Map;
        }

        public string GetServerIp()
        {
            return ServerIp;
        }

        public int GetServerQueryPort()
        {
            return QueryPort;
        }

        public int GetServerPort()
        {
            return Port;
        }

        public bool GetServerRconEnabeld()
        {
            return RconEnabled;
        }

        public int GetServerRconPort()
        {
            return RconPort;
        }

        public int GetServerMaxPlayer()
        {
            return maxPlayer;
        }

        public string GetServerStartArgument()
        {
            return ServerStartArgument;
        }
    }
}
