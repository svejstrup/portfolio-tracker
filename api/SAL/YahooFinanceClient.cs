using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using api.models;
using NodaTime;
using YahooQuotesApi;

namespace api.SAL
{
    public class YahooFinanceClient
    {
        public YahooFinanceClient()
        {
            
        }

        public async Task<Dictionary<string, BaseStock>> GetCurrentPrices(List<string> symbols) 
        {
            if (!symbols.Any())
                return new Dictionary<string, BaseStock>();

            YahooQuotes yahooQuotes = new YahooQuotesBuilder().Build();
            Dictionary<string, Security?> securities = await yahooQuotes.GetAsync(symbols.ToArray());
            
            var res = securities
                .Select(sec => new BaseStock 
                {
                    Symbol = sec.Key,
                    Price = sec.Value.RegularMarketPrice.HasValue ? Decimal.ToDouble(sec.Value.RegularMarketPrice.Value) : null,
                    ChangeToday = 1 + (Decimal.ToDouble(sec.Value.RegularMarketChangePercent.Value) / 100)
                })
                .ToDictionary(sp => sp.Symbol);

            return res;
        }

        public async Task<double> GetHistoricPrice(string symbol, DateTimeOffset date)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return 0;

            YahooQuotes yahooQuotes = new YahooQuotesBuilder()
                .HistoryStarting(Instant.FromDateTimeOffset(date))
                .Build();
     
            Security security = await yahooQuotes.GetAsync(symbol, HistoryFlags.PriceHistory)
                ?? throw new ArgumentException("Unknown symbol.");

            var priceHistory = security.PriceHistory.Value;
            var tick = priceHistory[0];


            return tick.Close;
        }
    }
}