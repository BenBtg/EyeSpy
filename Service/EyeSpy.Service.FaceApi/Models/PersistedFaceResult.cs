using Newtonsoft.Json;

namespace EyeSpy.Service.FaceApi.Models
{
    public class PersistedFaceResult
    {
        [JsonProperty("persistedFaceId")]
        public string PersistedFaceId { get; set; }
    }
}