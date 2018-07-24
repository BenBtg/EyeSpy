using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class BaseDetection
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}