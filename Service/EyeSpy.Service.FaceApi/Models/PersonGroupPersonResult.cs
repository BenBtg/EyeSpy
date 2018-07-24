using Newtonsoft.Json;
using System.Collections.Generic;

namespace EyeSpy.Service.FaceApi.Models
{
    public class PersonGroupPersonResult : BasePersonGroupPersonResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("persistedFaceIds")]
        public List<string> PersistedFaceIds { get; set; }
    }
}