using EyeSpy.Service.Common.Models;
using EyeSpy.Service.Common.Abstractions;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Collections.Generic;
using EyeSpy.Service.AzureStorage.Models;

namespace EyeSpy.Service.AzureStorage.Services
{
    public class AzureTrustedPersonsStorage : ITrustedPersonsStorage
    {
        private readonly string storageAccountName;
        private readonly string storageAccessKey;
        private const string KnownPersonsContainerName = "kpcontainer";
        private const string KnownPersonsTableName = "kptable";
        private const string DetectionsContainerName = "dtcontainer";
        private const string DetectionsTableName = "dttable";
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

        public async Task<TrustedPerson> CreateTrustedPersonAsync(BaseTrustedPerson trustedPerson, byte[] trustedPersonImageData)
        {
            // Upload person image to known persons blob container
            var knownPersonImageUrl = await this.blobStorageService.UploadBytesToContainerAsync(trustedPerson.Id, trustedPersonImageData, KnownPersonsContainerName);

            // Enter record in known persons table
            var trustedPersonResult = new TrustedPerson
            {
                Id = trustedPerson.Id,
                Name = trustedPerson.Name,
                ProfileUrl = knownPersonImageUrl
            };

            var success = await this.tableStorageService.CreateEntityInTableAsync<TrustedPersonEntity>(TrustedPersonEntity.FromTrustedPerson(trustedPersonResult), KnownPersonsTableName);

            //if (!success)
                // TODO: Roll back changes

            return trustedPersonResult;
        }

        public Task<TrustedPerson> GetTrustedPersonByIdAsync(string id)
        {
            return Task.FromResult<TrustedPerson>(null);
        }

        public Task<IList<TrustedPerson>> GetTrustedPersonsAsync()
        {
            return Task.FromResult<IList<TrustedPerson>>(null);
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
            await this.blobStorageService.CreateContainerIfNotExistsAsync(KnownPersonsContainerName);

            // Create the 'detections' blob container if it does not exist    
            await this.blobStorageService.CreateContainerIfNotExistsAsync(DetectionsContainerName);

            // Create the 'known persons' table if it does not exist
            await this.tableStorageService.CreateTableIfNotExistsAsync(KnownPersonsTableName);

            // Create the 'detections' table if it does not exist
            await this.tableStorageService.CreateTableIfNotExistsAsync(DetectionsTableName);

            this.initialized = true;
        }        
    }
}