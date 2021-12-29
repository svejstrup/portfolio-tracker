using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.entities;
using api.SAL;

namespace api.DAL
{
    public class CurrencyDataManager
    {
        private readonly TableStorageDataManager<CurrencyEntity> _tableStorageDataManager;
        private readonly CurrencyClient _currencyClient;


        public CurrencyDataManager(CurrencyClient currencyClient)
        {
            _tableStorageDataManager = new TableStorageDataManager<CurrencyEntity>("Currency");
            _currencyClient = currencyClient;
        }

        public async Task InsertMany(IEnumerable<CurrencyEntity> exchangeRates)
        {
            await _tableStorageDataManager.BatchInsert(exchangeRates);
        }


        public async Task<Dictionary<string, double>> GetLatest(HashSet<string> currencies)
        {
            var latest = await _tableStorageDataManager.GetLatestByPartitionKey(currencies);

            var toUpdate = latest
                .Where(ce => DateTimeOffset.UtcNow - ce.Timestamp > TimeSpan.FromDays(1))
                .Select(ce => ce.Currency)
                .Union(
                    currencies.Except(latest.Select(ce => ce.Currency))
                ).ToList();

            if (toUpdate.Any())
            {
                var exchangeRates = await _currencyClient.GetExchangeRates(toUpdate);
                var entities = exchangeRates
                    .Select(rate => new CurrencyEntity(rate.Key, rate.Value, DateTimeOffset.UtcNow));
                
                await InsertMany(entities);
                latest = await _tableStorageDataManager.GetLatestByPartitionKey(currencies);
            }

            var keyValuePairs = latest.Select(c => new KeyValuePair<string, double>(c.Currency, c.ExchangeRate));

            return new Dictionary<string, double>(keyValuePairs);
        }

    }
}