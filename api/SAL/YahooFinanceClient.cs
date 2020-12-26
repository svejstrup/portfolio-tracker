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
            var securities = await Yahoo.Symbols(symbols.ToArray()).Fields(Field.Symbol, Field.RegularMarketPrice, Field.Currency).QueryAsync();
            
            var res = securities.Values
                .Select(sec => new BaseStock 
                {
                    Symbol = sec.Symbol,
                    Currency = sec.Currency,
                    Price = sec.RegularMarketPrice
                })
                .ToDictionary(sp => sp.Symbol);

            return res;
        }
    }
}