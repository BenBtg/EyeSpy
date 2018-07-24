using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace EyeSpy.Service.AzureStorage.Services
{
    public class AzureTableStorageService
    {
        private readonly CloudStorageAccount cloudStorageAccount;
        private readonly CloudTableClient tableClient;

        public AzureTableStorageService(CloudStorageAccount cloudStorageAccount)
        {
            this.cloudStorageAccount = cloudStorageAccount ?? throw new ArgumentNullException($"{nameof(cloudStorageAccount)} cannot be null");
            this.tableClient = this.cloudStorageAccount.CreateCloudTableClient();
        }

        public async Task<bool> CreateTableIfNotExistsAsync(string tableName)
        {
            CloudTable table = this.tableClient.GetTableReference(tableName);
            return await table.CreateIfNotExistsAsync();
        }

        public async Task<bool> CreateEntityInTableAsync<T>(T entity, string tableName) where T : TableEntity
        {
            CloudTable table = this.tableClient.GetTableReference(tableName);
            TableOperation insertOperation = TableOperation.InsertOrReplace(entity);

            try
            {
                await table.ExecuteAsync(insertOperation);
                return true;
            }
            catch
            {
                // TODO: Log exception
            }

            return false;
        }
    }
}