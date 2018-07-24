using EyeSpy.Service.Common.Models;
using EyeSpy.Service.Common.Abstractions;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;

namespace EyeSpy.Service.AzureStorage.Services
{
    public class AzureTrustedPersonsStorage : ITrustedPersonsStorage
    {
        private readonly string storageAccountName;
        private readonly string storageAccessKey;
        private const string KnownPersonsContainerName = "kpcontainer";
        private const string KnownPersonsTableName = "kptable";
        private bool initialized;
        private CloudStorageAccount cloudStorageAccount;
        private AzureBlobStorageService blobStorageService;
        private AzureTableStorageService tableStorageService;

        public AzureTrustedPersonsStorage(string storageAccountName, string storageAccessKey)
        {
            if (string.IsNullOrWhiteSpace(storageAccountName) || string.IsNullOrWhiteSpace(storageAccessKey))
                throw new ArgumentNullException($"{nameof(storageAccountName)} and {nameof(storageAccessKey)} must have values assigned");

            this.storageAccountName = storageAccountName;
            this.storageAccessKey = storageAccessKey;
            this.InitAsync().Wait();
        }

        public Task<bool> CreateTrustedPersonAsync(TrustedPerson trustedPerson, byte[] trustedPersonImageData)
        {
            return Task.FromResult<bool>(true);
        }

        private async Task InitAsync()
        {
            if (this.initialized)
                return;

            // Initialize the cloud storage account
            this.cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(this.storageAccountName, this.storageAccessKey), true);

            // Initialize foundational Azure Storage services
            this.blobStorageService = new AzureBlobStorageService(this.cloudStorageAccount);
            this.tableStorageService = new AzureTableStorageService(this.cloudStorageAccount);

            // Create the 'known persons' blob container if it does not exist       
            var success = await this.blobStorageService.CreateContainerIfNotExistsAsync(KnownPersonsContainerName);

            if (!success)
                throw new Exception($"Unable to initialize the {KnownPersonsContainerName} Azure Blob Container");

            // Create the 'known persons' table if it does not exist
            success = await this.tableStorageService.CreateContainerIfNotExistsAsync(KnownPersonsTableName);

            if (!success)
                throw new Exception($"Unable to initialize the {KnownPersonsTableName} Storage Table");
            this.initialized = true;
        }        
    }
}