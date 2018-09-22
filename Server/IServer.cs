using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
   public class IServer : IEqualityComparer<IServer>
    {
        public string ServerName { get; set; }
        public string ArkSurvivalFolder { get; set; }
        public string Map { get; set; }
        public string ServerIp { get; set; }
        public string ServerStartArgument { get; set; }
        public int QueryPort { get; set; }
        public int Port { get; set; }
        public bool RconEnabled { get; set; }
        public int RconPort { get; set; }
        public int maxPlayer { get; set; }

        public IServer(string key)
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
        }


        public IServer()
        {
        }

        public bool Equals(IServer x, IServer y)
        {
            return x.ServerName == y.ServerName;
        }

        public int GetHashCode(IServer obj)
        {
            IServer se = (IServer)obj;
            return se.ServerName.GetHashCode();
        }
    }
}
