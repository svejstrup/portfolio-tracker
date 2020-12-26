using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.models.entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace api.DAL
{
    public class TransactionDataManager
    {
        private readonly TableStorageDataManager _tableStorageDataManager;

        public TransactionDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager("Transactions");
        }

        public async Task<Portfolio> GetPortfolioFromTransactions()
        {   
            var transactions = await _tableStorageDataManager.GetAll<TransactionEntity>();

            return new Portfolio();
        }

        public async Task InsertMany(List<TransactionEntity> entities)
        {
            await _tableStorageDataManager.BatchInsert(entities.Cast<ITableEntity>());
        }
    }
}