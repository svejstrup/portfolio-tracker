using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using api.models;
using YahooFinanceApi;

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

            var securities = await Yahoo
                .Symbols(symbols.ToArray())
                .Fields(Field.Symbol, Field.RegularMarketPrice, Field.RegularMarketChangePercent)
                .QueryAsync();
            
            var res = securities.Values
                .Select(sec => new BaseStock 
                {
                    Symbol = sec.Symbol,
                    Price = sec.RegularMarketPrice,
                    ChangeToday = 1 + (sec.RegularMarketChangePercent / 100)
                })
                .ToDictionary(sp => sp.Symbol);

            return res;
        }
    }
}