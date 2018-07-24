﻿using Newtonsoft.Json;

namespace ExeSpy.Service.Common.Models
{
    public class TrustedPerson
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("profileUrl")]
        public string ProfileUrl { get; set; }
    }
}