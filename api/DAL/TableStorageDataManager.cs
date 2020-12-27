using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace api.DAL
{
    public class TableStorageDataManager<TEntityType> where TEntityType : ITableEntity, new()
    {
         private readonly CloudTable _table;
        private const int MaxBatchSize = 100; // Table storage does not allow batches larger than 100 

        public TableStorageDataManager(string tableName)
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(tableName);

            // Create table if it does not already exist
            _table.CreateIfNotExistsAsync().Wait();
        }

        public async Task BatchInsert(IEnumerable<ITableEntity> entityList)
        {
            var batchOperation = new TableBatchOperation();

            // Insert entities in batches of at most 'maxBatchSize'
            foreach (var entity in entityList)
            {
                batchOperation.InsertOrMerge(entity);

                if (batchOperation.Count == MaxBatchSize)
                {
                    var res = await _table.ExecuteBatchAsync(batchOperation);
                    batchOperation.Clear();
                }
            }

            // Handle last smaller batch
            if (batchOperation.Count > 0)
            {
                await _table.ExecuteBatchAsync(batchOperation);
            }
        }

        public async Task<List<TEntityType>> GetAll()
        {
            var query = new TableQuery<TEntityType>();
            TableContinuationToken token = null;
            List<TEntityType> entities = new List<TEntityType>();

            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(segment.Results);
                token = segment.ContinuationToken;
            } while (token != null);
            

            return entities;
        }

        // public async Task<List<LogTransEntity>> GetDataByDeviceIdAndDate(string partitionKey, DateTime fromDate, DateTime toDate, string deviceDataType)
        // {
        //     var entities = new List<LogTransEntity>();
        //     TableContinuationToken token = null;
        //     if (deviceDataType == "NitrateLogTrans")
        //     {
        //         TableQuery<NitrateLogTransEntity> rangeQuery = new TableQuery<NitrateLogTransEntity>()
        //             .Where(
        //                 TableQuery.CombineFilters(
        //                     TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
        //                     TableOperators.And,
        //                     TableQuery.CombineFilters(
        //                         TableQuery.GenerateFilterConditionForDate("MeasurementTime", QueryComparisons.GreaterThanOrEqual, fromDate),
        //                         TableOperators.And,
        //                         TableQuery.GenerateFilterConditionForDate("MeasurementTime", QueryComparisons.LessThanOrEqual, toDate))));
        //         do
        //         {
        //             var queryResult = await _cloudTable.ExecuteQuerySegmentedAsync(rangeQuery, token);
        //             entities.AddRange(queryResult.Results);
        //             token = queryResult.ContinuationToken;
        //         } while (token != null);
        //     }
        //     else if (deviceDataType == "NitrateNitrogenLogTrans")
        //     {
        //         TableQuery<NitrateNitrogenLogTransEntity> rangeQuery = new TableQuery<NitrateNitrogenLogTransEntity>()
        //             .Where(
        //                 TableQuery.CombineFilters(
        //                     TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
        //                     TableOperators.And,
        //                     TableQuery.CombineFilters(
        //                         TableQuery.GenerateFilterConditionForDate("MeasurementTime", QueryComparisons.GreaterThanOrEqual, fromDate),
        //                         TableOperators.And,
        //                         TableQuery.GenerateFilterConditionForDate("MeasurementTime", QueryComparisons.LessThanOrEqual, toDate))));
        //         do
        //         {
        //             var queryResult = await _cloudTable.ExecuteQuerySegmentedAsync(rangeQuery, token);
        //             entities.AddRange(queryResult.Results);
        //             token = queryResult.ContinuationToken;
        //         } while (token != null);
        //     }

        //     return entities;
        // }
    
    }
}