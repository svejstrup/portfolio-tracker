using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api.SAL
{
    public class CurrencyClient
    {
        private readonly HttpClient _httpClient;
        private const string _baseUrl = "https://free.currconv.com/api/v7/convert?q=[TO]_DKK";
        private const string _urlSuffix = "&compact=ultra&apiKey=8405b2040f1a13271a07";

        public CurrencyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string BuildQueryUrl(string currency) 
        {
            return $"{_baseUrl.Replace("[TO]", currency)}{_urlSuffix}";
        }

        public async Task<Dictionary<string, double>> GetExchangeRates(IEnumerable<string> currencies)
        {
            var exchangeRateMap = new Dictionary<string, double>();

            foreach (var currency in currencies)
            {
                var query = BuildQueryUrl(currency);

                var response = await _httpClient.GetAsync(query);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var exchangeRateString = json.Split(':').Last().Replace("}", string.Empty);

                if (double.TryParse(exchangeRateString, NumberStyles.Any, CultureInfo.InvariantCulture, out var exchangeRate))
                {
                    exchangeRateMap.Add(currency, exchangeRate);
                }
            }

            return exchangeRateMap;
        }
    }
}