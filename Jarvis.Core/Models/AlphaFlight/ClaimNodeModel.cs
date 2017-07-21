using System.Runtime.Serialization;

namespace Jarvis.Core.Models.AlphaFlight
{
    [DataContract]
    internal class ClaimNodeModel
    {
        [DataMember(Name = "zone")]
        public int Zone { get; set; }

        [DataMember(Name = "node")]
        public int Node { get; set; }

        [DataMember(Name = "user")]
        public string UserId { get; set; }
    }
}
