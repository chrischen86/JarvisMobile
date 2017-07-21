using System.Runtime.Serialization;

namespace Jarvis.Core.Models.AlphaFlight
{
    [DataContract]
    public class Strike
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }

        [DataMember(Name = "node_id")]
        public string NodeId { get; set; }

        [DataMember(Name = "node")]
        public Node Node { get; set; }
    }
}
