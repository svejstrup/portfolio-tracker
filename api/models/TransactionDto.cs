using System;
using api.util;

namespace api.models
{
    public class TransactionDto
    {
        public string OrderNumber {get; set;}
        public string Name {get; set;}
        public TransactionType TransactionType {get; set;}
        public int Pieces {get; set;}
        public double Price {get; set;}
        public double Fee {get; set;}
        public string Symbol {get; set;}
        public string Currency {get; set;}
        public double ExchangeRate {get; set;}
        public DateTimeOffset Date {get; set;}
        public StockExchange StockExchange {get; set;}
    }
}