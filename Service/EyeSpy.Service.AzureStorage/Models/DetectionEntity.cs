using System;
using System.Linq;
using EyeSpy.Service.Common.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace EyeSpy.Service.AzureStorage.Models
{
    public class DetectionEntity : TableEntity
    {
        public DetectionEntity(string id)
        {
            // NOTE: Short-term: using first character of the id as the partition key
            this.PartitionKey = id.First().ToString();
            this.RowKey = id;
        }

        public DetectionEntity() { }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public static DetectionEntity FromDetection(Detection detection)
        {
            DateTimeOffset timestamp;
            DateTimeOffset.TryParse(detection.DetectionTimestamp, out timestamp);

            return new DetectionEntity(detection.Id) { ImageUrl = detection.DetectionImageUrl, Timestamp = timestamp };
        }

        public Detection ToDetection()
        {
            return new Detection { Id = this.RowKey, DetectionImageUrl = this.ImageUrl, DetectionTimestamp = this.Timestamp.ToString() };
        }
    }
}