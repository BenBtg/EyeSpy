using System.Collections.Generic;
using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    public class FaceIdentifyResponse : List<FaceIdentifyResult>
    {
    }

    public class FaceIdentifyResult
    {
        [JsonProperty("faceId")]
        public string FaceId { get; set; }

        [JsonProperty("candidates")]
        public List<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("personId")]
        public string PersonId { get; set; }

        [JsonProperty("confidence")]
        public float Confidence { get; set; }
    }
}