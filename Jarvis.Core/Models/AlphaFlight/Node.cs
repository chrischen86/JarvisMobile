using Android.Runtime;
using Jarvis.Core.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Jarvis.Core.Models.AlphaFlight
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class Node
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "node")]
        public int NodeNumber { get; set; }

        [DataMember(Name = "is_reserved")]
        [JsonConverter(typeof(StringBooleanConverter))]
        public bool IsReserved { get; set; }
    }
}
