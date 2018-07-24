using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    [JsonObject("modelTrainingResult")]
    public class ModelTrainingResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonProperty("lastActionDateTime")]
        public string LastActionDateTime { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }
    }
}