using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    public class PersonGroupPerson
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}