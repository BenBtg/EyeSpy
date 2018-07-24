using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class TrustedPerson : BaseTrustedPerson
    {     
        [JsonProperty("imageReference")]
        public string ImageReference { get; set; }
    }   
}