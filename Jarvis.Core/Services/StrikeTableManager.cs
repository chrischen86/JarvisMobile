using Jarvis.Core.Api.AlphaFlight;
using Jarvis.Core.Models.AlphaFlight;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jarvis.Core.Services
{
    public class StrikeTableManager
    {
        IStrikeTableApi Api;

        public StrikeTableManager(IStrikeTableApi api)
        {
            Api = api;
        }

        public StrikeTableManager()
        {
            Api = new StrikeTableApi();
        }

        public Task<List<Zone>> GetMappingAsync()
        {
            return Api.GetMapping();
        }

        public Task<Strike> ClaimNode(int zone, int node, string userId)
        {
            return Api.ClaimNode(zone, node, userId);
        }
    }
}
