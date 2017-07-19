﻿using System.Runtime.Serialization;

namespace Jarvis.Core.Models
{
    [DataContract]
    public class Identity
    {
        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "team")]
        public Team Team { get; set; }

        [DataMember(Name = "ok")]
        public bool IsAuthenticated { get; set; }
    }
}
