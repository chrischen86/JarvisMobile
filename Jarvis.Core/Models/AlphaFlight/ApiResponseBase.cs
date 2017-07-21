using Android.Runtime;
using System.Runtime.Serialization;

namespace Jarvis.Core.Models.AlphaFlight
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ApiResponseBase
    {
        [DataMember(Name = "error")]
        public bool IsError { get; set; }

        [DataMember(Name = "message")]
        public string ErrorMessage { get; set; }
    }
}
