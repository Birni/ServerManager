using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagerGUI.SteamWeb
{
    class SteamWebInterface
    {
        private const string msteamWebApiBaseUrl = "https://api.steampowered.com/";
        private string msteamWebApiKey; 

        public SteamWebInterface(string steamWebApiKey)
        {
            if (String.IsNullOrWhiteSpace(steamWebApiKey))
            {
                throw new ArgumentNullException("steamWebApiKey");
            }
            else
            {
                msteamWebApiKey = steamWebApiKey;
            }
        }
        public bool isServerRunning(string serverIp, int querryPort)
        {

            return true; 
        }



    }
}
