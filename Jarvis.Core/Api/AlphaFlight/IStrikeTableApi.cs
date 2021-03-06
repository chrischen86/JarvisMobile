﻿using Jarvis.Core.Models.AlphaFlight;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jarvis.Core.Api.AlphaFlight
{
    public interface IStrikeTableApi
    {
        Task<List<Zone>> GetMapping();
        Task<Strike> ClaimNode(int zone, int node, string userId);
    }
}