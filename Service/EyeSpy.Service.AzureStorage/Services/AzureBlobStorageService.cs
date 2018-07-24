using System;
using System.IO;
using System.Threading.Tasks;
using EyeSpy.Service.Common;
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

        public async Task<string> UploadStreamToContainerAsync(string blobId, Stream streamSource, string containerName)
        {
            var blobReference = this.GetBlockBlobReferenceForContainer(blobId, containerName);
            await blobReference.UploadFromStreamAsync(streamSource);
            return this.PrepareFriendlyBlobReference(blobReference);
        }

        public async Task<string> UploadBytesToContainerAsync(string blobId, byte[] buffer, string containerName)
        {
            var blobReference = this.GetBlockBlobReferenceForContainer(blobId, containerName);
            await blobReference.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
            return this.PrepareFriendlyBlobReference(blobReference);
        }       

        public async Task<byte[]> RetrieveBytesFromContainerAsync(string blobId, string containerName)
        {
            byte[] bytes = null;

            using (var memoryStream = new MemoryStream())
            {
                var blobReference = this.GetBlockBlobReferenceForContainer(blobId, containerName);
                await blobReference.DownloadToStreamAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        private CloudBlockBlob GetBlockBlobReferenceForContainer(string blobId, string containerName)
        {
            CloudBlobContainer container = this.blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlobReference = container.GetBlockBlobReference(blobId);
            return blockBlobReference;
        }

        private string PrepareFriendlyBlobReference(CloudBlockBlob blobReference)
        {
            return $"path={blobReference.Container.Name}&name={blobReference.Name}";
        }
    }
}