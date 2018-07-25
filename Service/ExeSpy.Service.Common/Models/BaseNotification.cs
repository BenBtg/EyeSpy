using Newtonsoft.Json;

namespace EyeSpy.Service.Common.Models
{
    public class BaseNotification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}