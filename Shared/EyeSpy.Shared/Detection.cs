using System;

namespace EyeSpy.Shared
{
    public class Detection
    {
        public string Id { get; set; }
        public DateTime? DetectionTimestamp { get; set; }
        public string ImageReference { get; set; }
        public string DetectionImageUrl { get; set; }
    }
}