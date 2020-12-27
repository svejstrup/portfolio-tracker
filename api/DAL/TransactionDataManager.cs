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
        private readonly TableStorageDataManager<TransactionEntity> _tableStorageDataManager;
        private readonly Dictionary<string, string> _symbolTranslation;

        public TransactionDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager<TransactionEntity>("Transactions");
            _symbolTranslation = new Dictionary<string, string>() {
                {"BAVA", "BAVA.CO"}
            };
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
                    t.Symbol = _symbolTranslation.GetValueOrDefault(t.Symbol) ?? t.Symbol;
                    return t;
                }).ToList();
        }
    }
}