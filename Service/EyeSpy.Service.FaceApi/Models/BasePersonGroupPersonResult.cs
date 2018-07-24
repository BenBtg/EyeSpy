using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    public class BasePersonGroupPersonResult
    {
        [JsonProperty("personId")]
        public string PersonId { get; set; }
    }
}