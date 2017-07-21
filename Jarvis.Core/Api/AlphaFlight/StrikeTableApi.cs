using Jarvis.Core.Models.AlphaFlight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jarvis.Core.Api.AlphaFlight
{
    internal class StrikeTableApi : IStrikeTableApi
    {
        private readonly string Uri = "http://projectr.ca/slack/Friday/web/ConquestApi.php/activezones";
        private HttpClient client;

        public StrikeTableApi()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<List<Zone>> GetMapping()
        {
            var uri = new Uri(Uri);
            var response = await client.GetAsync(uri);

            List<Zone> zones = new List<Zone>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                zones = JsonConvert.DeserializeObject<List<Zone>>(content);
            }

            return zones;
        }
    }
}
