using System.Runtime.Serialization;

namespace Jarvis.Core.Models
{
    [DataContract]
    public class Team
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "domain")]
        public string Domain { get; set; }

        [DataMember(Name = "image_88")]
        public string AvatarUri { get; set; }
    }
}
