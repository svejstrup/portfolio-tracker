using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using api.models.entities;
using api.util;
using Microsoft.WindowsAzure.Storage.Table;

namespace api.DAL
{
    public class TransactionDataManager
    {
        private readonly TableStorageDataManager<TransactionEntity> _tableStorageDataManager;

        public TransactionDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager<TransactionEntity>("Transactions");
           
        }

        public async Task InsertMany(List<TransactionEntity> entities)
        {
            await _tableStorageDataManager.BatchInsert(entities.Cast<ITableEntity>());
        }

        public async Task<List<TransactionEntity>> GetAll()
        {
            return (await _tableStorageDataManager.GetAll())
                .Select(t => 
                {
                    t.Symbol = SymbolTranslation.TranslateSymbol(t);
                    return t;
                }).ToList();
        }
    }
}