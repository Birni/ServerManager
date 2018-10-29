using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SteamWeb.Models;

namespace SteamWeb
{

    public class SteamWebInterface 
    {
        private const string steamWebApiBaseUrl = "https://api.steampowered.com/";
        private string steamWebApiKey = SteamWebDataInterface.MSteamWebDataInterface.ApiKey;
        private readonly SteamWebRequest steamWebRequest;

        public SteamWebInterface()
        {
            this.steamWebRequest = steamWebRequest == null
                ? new SteamWebRequest(steamWebApiBaseUrl, steamWebApiKey)
                : steamWebRequest;
        }

        public SteamWebInterface(string steamWebApiBaseUrl, string steamWebApiKey)
        {
            if (String.IsNullOrWhiteSpace(steamWebApiKey))
            {
                throw new ArgumentNullException("steamWebApiKey");
            }


            this.steamWebRequest = steamWebRequest == null
                ? new SteamWebRequest(steamWebApiBaseUrl, steamWebApiKey)
                : steamWebRequest;


        }


        public async Task<SteamWebResponse<T>> GetAsync<T>(string interfaceName, string methodName, int version, IList<SteamWebRequestParameter> parameters = null)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(methodName));
            Debug.Assert(version > 0);

            return await steamWebRequest.GetAsync<T>(interfaceName, methodName, version, parameters);
        }


        public async Task<SteamWebResponse<T>> PostAsync<T>(string interfaceName, string methodName, int version, IList<SteamWebRequestParameter> parameters = null)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(methodName));
            Debug.Assert(version > 0);

            return await steamWebRequest.PostAsync<T>(interfaceName, methodName, version, parameters);
        }


        public async Task<PublishedFileDetailsResultContainer> SteamWebGetPublishedFileDetails(string AppId )
        {
            var parameters = new List<SteamWebRequestParameter>
            {
                new SteamWebRequestParameter("itemcount", "1"),
                new SteamWebRequestParameter("publishedfileids[0]", AppId)
            };


            var steamWebResponse = await PostAsync<PublishedFileDetailsResultContainer>("ISteamRemoteStorage", "GetPublishedFileDetails", 1, parameters);

            return steamWebResponse.Data;

        }

        public async Task<SchemaForGameResultContainer> SchemaForGameResultContainer(string AppId)
        {
            var parameters = new List<SteamWebRequestParameter>
            {
                new SteamWebRequestParameter("appid", AppId)
            };


            var steamWebResponse = await GetAsync<SchemaForGameResultContainer>("ISteamUserStats", "GetSchemaForGame", 1, parameters);

            return steamWebResponse.Data;

        }
    }
}