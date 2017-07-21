using Jarvis.Core.Models.AlphaFlight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Core.Api.AlphaFlight
{
    internal class StrikeTableApi : IStrikeTableApi
    {
        private readonly string Uri = "http://projectr.ca/slack/Friday/web/ConquestApi.php/activezones";
        private readonly string AttackUri = "http://projectr.ca/slack/Friday/web/ConquestApi.php/attack";
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

        public async Task<Strike> ClaimNode(int zone, int node, string userId)
        {
            var uri = new Uri(AttackUri);

            var claimModel = new ClaimNodeModel
            {
                Zone = zone,
                Node = node,
                UserId = userId,
            };

            var body = new StringContent(JsonConvert.SerializeObject(claimModel), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, body);

            Strike strike = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                strike = JsonConvert.DeserializeObject<Strike>(content);
            }
            return strike;
        }
    }
}
