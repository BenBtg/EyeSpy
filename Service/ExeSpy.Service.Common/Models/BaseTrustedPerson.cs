using Newtonsoft.Json;
namespace EyeSpy.Service.Common.Models
{
    public class BaseTrustedPerson
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}