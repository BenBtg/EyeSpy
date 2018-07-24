using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class Detection : BaseDetection
    {
        [JsonProperty("detectionImageUrl")]
        public string DetectionImageUrl { get; set; }

        [JsonProperty("detectionTimestamp")]
        public string DetectionTimestamp { get; set; }
    }
}