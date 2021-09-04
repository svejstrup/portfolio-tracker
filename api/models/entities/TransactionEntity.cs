using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace api.models.entities
{
    public class TransactionEntity : TableEntity, INordeaExportFormat
    {
        public TransactionEntity()
        {
        }

        public TransactionEntity(NordeaExportFormat exportData)
        {
            PartitionKey = exportData.Name.Replace("/", string.Empty);
            RowKey = $"{exportData.OrderNumber}:{exportData.OrderType}:{exportData.Date.ToUnixTimeSeconds()}";

            foreach(var prop in typeof(NordeaExportFormat).GetProperties().Where(p => p.CanWrite))
            {
                var thisProp = this.GetType().GetProperty(prop.Name);
                thisProp.SetValue(this, prop.GetValue(exportData));
            }
        }

        public TransactionEntity(TransactionDto transactionDto)
        {
            OrderNumber = transactionDto.OrderNumber;
            Name = transactionDto.Name.Trim();
            OrderType = transactionDto.TransactionType.ToString();
            Pieces = transactionDto.Pieces;
            Price = transactionDto.Price;
            Fee = transactionDto.Fee;
            Symbol = transactionDto.Symbol;
            Currency = transactionDto.Currency;
            ExchangeRate = transactionDto.ExchangeRate;
            Date = transactionDto.Date;
            StockExchange = transactionDto.StockExchange.ToString();
            TotalAmount = (transactionDto.Pieces * Price * ExchangeRate) + Fee;
        
            PartitionKey = Name.Replace("/", string.Empty);
            RowKey = $"{OrderNumber}:{OrderType}:{Date.ToUnixTimeSeconds()}";
        }

        public TransactionEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

        public string OrderNumber {get; set;}
        public string Name {get; set;}
        public string OrderType {get; set;}
        public int? Pieces {get; set;}
        public double Price {get; set;}
        public string Account {get; set;}
        public double Fee {get; set;}
        public string Symbol {get; set;}
        public string Currency {get; set;}
        public double ExchangeRate {get; set;}
        public DateTimeOffset Date {get; set;}
        public string StockExchange { get; set; }
        public double TotalAmount {get; set;}
    }
}