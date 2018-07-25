using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class DetectionNotification : BaseNotification
    {
        [JsonProperty("detectionId")]
        public string DetectionId { get; set; }

        [JsonProperty("imageReference")]
        public string ImageReference { get; set; }
    }
}