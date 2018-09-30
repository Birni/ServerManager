using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamWeb
{

    public class SteamWebHttpClient
    {

        public async Task<HttpResponseMessage> GetAsync(string command)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(command));

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync(command);

            response.EnsureSuccessStatusCode();

            if (response.Content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string command, HttpContent content)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(command));

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsync(command, content);

            response.EnsureSuccessStatusCode();

            if (response.Content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return response;
        }
    }
}