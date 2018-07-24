using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class TrustedPerson : BaseTrustedPerson
    {     
        [JsonProperty("profileUrl")]
        public string ProfileUrl { get; set; }
    }   
}