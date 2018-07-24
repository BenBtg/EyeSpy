using System.Collections.Generic;
using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    public class FaceIdentifyRequest
    {
        [JsonProperty("faceIds")]
        public List<string> FaceIds { get; set; }

        [JsonProperty("personGroupId")]
        public string PersonGroupId { get; set; }

        [JsonProperty("maxNumOfCandidatesReturned")]
        public int MaxNumOfCandidatesReturned { get; set; }

        [JsonProperty("confidenceThreshold")]
        public float ConfidenceThreshold { get; set; }
    }
}