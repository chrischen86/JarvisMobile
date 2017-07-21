using Android.Runtime;
using Jarvis.Core.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jarvis.Core.Models.AlphaFlight
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class Zone
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "conquest_id")]
        public string ConquestId { get; set; }

        [DataMember(Name = "zone")]
        public int ZoneNumber { get; set; }

        [DataMember(Name = "battle_count")]
        public int BattleCount { get; set; }

        [DataMember(Name = "is_owned")]
        [JsonConverter(typeof(StringBooleanConverter))]
        public bool IsOwned { get; set; }

        [DataMember(Name = "strikes")]
        public List<Strike> Strikes { get; set; }
    }
}
