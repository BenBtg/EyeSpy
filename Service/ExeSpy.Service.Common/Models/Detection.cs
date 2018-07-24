using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class Detection : BaseDetection
    {
        [JsonProperty("imageReference")]
        public string ImageReference { get; set; }

        [JsonProperty("detectionTimestamp")]
        public string DetectionTimestamp { get; set; }
    }
}