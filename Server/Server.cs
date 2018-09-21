using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerGUI
{
    class Server
    {
        string ServerName { get; set; }
        string ArkSurvivalFolder { get; set; }
        string Map { get; set; }
        string ServerIp { get; set; }
        int QueryPort { get; set; }
        int Port { get; set; }
        bool RconEnabled { get; set; }
        int RconPort { get; set; }
        int maxPlayer { get; set; }

        public Server(string name)
        {
            ServerName = name;
        }

        public void StartServer()
        {


        }

    }

}
