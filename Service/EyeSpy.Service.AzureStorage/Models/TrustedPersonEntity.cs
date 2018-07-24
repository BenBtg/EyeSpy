using System.Linq;
using EyeSpy.Service.Common.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace EyeSpy.Service.AzureStorage.Models
{
    public class TrustedPersonEntity : TableEntity
    {
        public TrustedPersonEntity(string id)
        {
            // NOTE: Short-term: using first character of the id as the partition key
            this.PartitionKey = id.First().ToString();
            this.RowKey = id;
        }

        public TrustedPersonEntity() { }

        public string Name { get; set; }

        public string ImageReference { get; set; }

        public static TrustedPersonEntity FromTrustedPerson(TrustedPerson trustedPerson)
        {
            return new TrustedPersonEntity(trustedPerson.Id) { Name = trustedPerson.Name, ImageReference = trustedPerson.ImageReference };
        }

        public TrustedPerson ToTrustedPerson()
        {
            return new TrustedPerson { Id = this.RowKey, Name = this.Name, ImageReference = this.ImageReference };
        }
    }
}