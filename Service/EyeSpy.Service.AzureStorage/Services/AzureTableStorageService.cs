using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<T>> RetrieveRecentEntitiesAsync<T>(string tableName) where T : TableEntity, new()
        {
            CloudTable table = this.tableClient.GetTableReference(tableName);

            var entities = new List<T>();

            TableQuery<T> query = new TableQuery<T>();
            query.FilterString = TableQuery.GenerateFilterConditionForDate("Timestamp", "ge", DateTimeOffset.UtcNow.Subtract(TimeSpan.FromMinutes(5)));
            var queryResult = await table.ExecuteQuerySegmentedAsync(query, null);
            entities.AddRange(queryResult.Results);

            return entities.OrderBy(i => i.Timestamp).ToList<T>();
        }

        public async Task<List<T>> RetrieveAllEntitiesAsync<T>(string tableName) where T : TableEntity, new()
        {
            CloudTable table = this.tableClient.GetTableReference(tableName);
            TableContinuationToken token = null;
            var entities = new List<T>();

            do
            {
                TableQuery<T> query = new TableQuery<T>();
                var queryResult = await table.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities as List<T>;
        }

        public async Task<T> RetrieveEntityByIdAsync<T>(string id, string tableName) where T : TableEntity, new()
        {
            CloudTable table = this.tableClient.GetTableReference(tableName);
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", "eq", id)).Take(1);
            var queryResult = await table.ExecuteQuerySegmentedAsync(query, null);

            return queryResult.Results.FirstOrDefault();
        }
    }
}