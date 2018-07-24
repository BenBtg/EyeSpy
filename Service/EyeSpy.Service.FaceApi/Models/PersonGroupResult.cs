using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    [JsonObject("personGroupResult")]
    public class PersonGroupResult : PersonGroup
    {
        [JsonProperty("personGroupId")]
        public string PersonGroupId { get; set; }

        [JsonProperty("userData")]
        public object UserData { get; set; }
    }
}