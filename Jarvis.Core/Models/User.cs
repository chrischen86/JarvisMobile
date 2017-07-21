using Android.Runtime;
using System.Runtime.Serialization;

namespace Jarvis.Core.Models
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class User
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name= "id")]
        public string Id { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "image_72")]
        public string AvatarUri { get; set; }
    }
}
