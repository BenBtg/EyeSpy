using Newtonsoft.Json;
using System.Collections.Generic;

namespace EyeSpy.Service.FaceApi.Models
{
    [JsonArray(AllowNullItems = true)]
    public class FaceDetectResponse : List<FaceDetectResult>
    {
    }

    public class FaceDetectResult
    {
        [JsonProperty("faceId")]
        public string FaceId { get; set; }

        [JsonProperty("faceRectangle")]
        public FaceRectangle FaceRectangle { get; set; }
    }

    public class FaceRectangle
    {
        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("left")]
        public int Left { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}