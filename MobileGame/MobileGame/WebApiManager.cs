using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MobileGame
{
    internal static class WebApiManager
    {
        internal static async Task<string> CreateGame(CookieContainer cookieJar)
        {
            var handler = new HttpClientHandler {CookieContainer = cookieJar};
            var hc = new HttpClient(handler) {BaseAddress = new Uri(Constants.EndPoint)};

            //HttpResponseMessage contents = await hc.GetAsync(new Uri(Url));
            var contents = await hc.GetStringAsync(Constants.CreateGameEndPoint).ConfigureAwait(false);
            //contents = await hc.

            var GameID = JsonConvert.DeserializeObject<string>(contents);
            return GameID;
        }
    }
}