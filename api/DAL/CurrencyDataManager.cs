using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.entities;

namespace api.DAL
{
    public class CurrencyDataManager
    {
        private readonly TableStorageDataManager<CurrencyEntity> _tableStorageDataManager;

        public CurrencyDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager<CurrencyEntity>("Currency");
        }

        public async Task InsertMany(IEnumerable<CurrencyEntity> exchangeRates)
        {
            await _tableStorageDataManager.BatchInsert(exchangeRates);
        }


        public async Task<Dictionary<string, double>> GetLatest(IEnumerable<string> currencies)
        {
            var latest = await _tableStorageDataManager.GetLatestByPartitionKey(currencies);

            var keyValuePairs = latest.Select(c => new KeyValuePair<string, double>(c.Currency, c.ExchangeRate));

            return new Dictionary<string, double>(keyValuePairs);
        }

    }
}