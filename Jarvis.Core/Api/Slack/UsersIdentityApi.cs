using Jarvis.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.Core.Api.Slack
{
    internal class UsersIdentityApi : IUsersIdentityApi
    {
        private readonly string Uri = "https://slack.com/api/users.identity";
        private HttpClient client;

        public UsersIdentityApi()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<Identity> GetIdentity(string token)
        {
            var uri = new Uri(string.Format("{0}?token={1}", Uri, token));

            var response = await client.GetAsync(uri);

            Identity identity = new Identity { IsAuthenticated = false };
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                identity = JsonConvert.DeserializeObject<Identity>(content);
            }

            return identity;
        }
    }
}
