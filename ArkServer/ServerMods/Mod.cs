using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamWeb;
using SteamWeb.Models;


namespace ArkServer.ServerMods
{
    public class Mod
    {
        public int ModId { get; set; }
        public string ModName { get; set; } = "unknown";
        public DateTime ModLastUpdate { get; set; } = DateTime.MinValue;

        public Mod()
        { }

        private async Task<Mod> InitializeAsync()
        {
            var details = new PublishedFileDetailsResultContainer();
            var steamweb = new SteamWebInterface();
            details = await steamweb.SteamWebGetPublishedFileDetails(ModId.ToString());
            if (!String.IsNullOrEmpty(details.Result.Details[0].Title))
            {
                ModName = details.Result.Details[0].Title;
                ModLastUpdate = details.Result.Details[0].TimeUpdated;
            }
            return this;
        }

        public static Task<Mod> CreateAsync(int modID)
        {
            var ret = new Mod();
            ret.ModId = modID;
            return ret.InitializeAsync();
        }

        public async Task GetModDeatilsFromSteamWeb()
        {
            var details = new PublishedFileDetailsResultContainer();
            var steamweb = new SteamWebInterface();
            details = await steamweb.SteamWebGetPublishedFileDetails(ModId.ToString());
            if (!String.IsNullOrEmpty(details.Result.Details[0].Title))
            {
                ModName = details.Result.Details[0].Title;
                ModLastUpdate = details.Result.Details[0].TimeUpdated;
            }
        }

        public async Task<bool> IsUpaded()
        {
            bool result = false;
            var details = new PublishedFileDetailsResultContainer();
            var steamweb = new SteamWebInterface();
            details = await steamweb.SteamWebGetPublishedFileDetails(ModId.ToString());

            if (!String.IsNullOrEmpty(details.Result.Details[0].Title))
            {
                if (null != details.Result.Details[0].TimeUpdated && ModLastUpdate != DateTime.MinValue && 
                    details.Result.Details[0].TimeUpdated > ModLastUpdate)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
