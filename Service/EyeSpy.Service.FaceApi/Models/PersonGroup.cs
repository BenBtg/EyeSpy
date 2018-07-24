using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{   
    [JsonObject("personGroup")]
    public class PersonGroup
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}