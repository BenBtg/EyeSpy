using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace EyeSpy.Service.AzureStorage.Services
{
    public class AzureBlobStorageService
    {
        private readonly CloudStorageAccount cloudStorageAccount;
        private readonly CloudBlobClient blobClient;

        public AzureBlobStorageService(CloudStorageAccount cloudStorageAccount)
        {
            this.cloudStorageAccount = cloudStorageAccount ?? throw new ArgumentNullException($"{nameof(cloudStorageAccount)} cannot be null");
            this.blobClient = this.cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<bool> CreateContainerIfNotExistsAsync(string containerName)
        {
            CloudBlobContainer container = this.blobClient.GetContainerReference(containerName);
            return await container.CreateIfNotExistsAsync();
        }
    }
}