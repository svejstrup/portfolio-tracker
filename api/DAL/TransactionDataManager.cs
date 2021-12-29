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
        private readonly Dictionary<string, string> _symbolTranslation;

        public TransactionDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager<TransactionEntity>("Transactions");
            _symbolTranslation = new Dictionary<string, string>() {
                {"CARLb", "CARL-B.CO"},
                {"MAERSKa", "MAERSK-A.CO"},
                {"NOVOb", "NOVO-B.CO"},
                {"SAS", "SAS-DKK.CO"},
                {"CSP1", "CSPX.AS"},
                {"AKERBP", "AKRBP.OL"},
                {"VOWG_p", "VOW3.DE"},
                {"", ""},
            };
        }

        private string TranslateSymbol(TransactionEntity entity)
        {
            // Some special cases isn't handled by the general rule of translation
            if (_symbolTranslation.ContainsKey(entity.Symbol))
                return _symbolTranslation[entity.Symbol];

            // Generally an identifier id appended to the symbol based on the stock exchange
            return entity.StockExchange switch
            {
                StockExchanges.OmxCopenhagen => entity.Symbol += ".CO",
                StockExchanges.OsloBors => entity.Symbol += ".OL",
                StockExchanges.Amsterdam => entity.Symbol += ".AS",
                StockExchanges.DeutcheBorse => entity.Symbol += ".DE",
                StockExchanges.Nasdaq => entity.Symbol,
                _ => entity.Symbol
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
                    t.Symbol = TranslateSymbol(t);
                    return t;
                }).ToList();
        }
    }
}