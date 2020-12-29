using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace api.models.entities
{
    public class CurrencyEntity : TableEntity
    {
        public CurrencyEntity()
        {
        }

        public CurrencyEntity(string currency, double exchangeRate, DateTimeOffset date)
        {
            PartitionKey = currency;
            RowKey = string.Format("{0:D19}", DateTimeOffset.MaxValue.Ticks - date.Ticks);

            Currency = currency;
            ExchangeRate = exchangeRate;
            Date = date;
        }

        public string Currency {get; set;}
        public double ExchangeRate {get; set;}
        public DateTimeOffset Date {get; set;}
    }
}