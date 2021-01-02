using System;
using System.Collections.Generic;
using System.Linq;
using api.models.entities;

namespace api.models
{
    public class Portfolio
    {
        public Portfolio()
        {
            CurrentHoldings = new List<Holding>();
            PreviousHoldings = new List<Holding>();
        }

        public string Name {get; set;}
        public List<Holding> CurrentHoldings {get; set;}
        public List<Holding> PreviousHoldings {get; set;}
        public double? CurrentValue {get => CurrentHoldings.Sum(h => h.TotalValue);}
        public double? TotalReturn {get => CurrentHoldings.Sum(h => h.ReturnValue) + PreviousHoldings.Sum(h => h.ReturnValue);}
        public double? TotalReturnPercentage {get => CurrentValue / (CurrentValue - TotalReturn);}
        public double? TodaysReturn 
        {
            get
            {
                return CurrentHoldings.Select(h => 
                {
                    var previousPrice = h.Price / h.ChangeToday;

                    return h.Price - previousPrice;
                })
                .Sum();
            }
        }
        public double? TodaysReturnPercentage {get => CurrentValue / (CurrentValue - TodaysReturn);}
    }

    public interface IStock
    {
        string Symbol {get; set;}
        string Name {get; set;}
        string Currency {get; set;}
        double? Price {get; set;}
        double? ChangeToday {get; set;}
        // double ExchangeRate {get; set;}
    }

    public class BaseStock : IStock
    {
        public string Symbol {get; set;}
        public string Name {get; set;}
        public string Currency {get; set;}
        public double? Price {get; set;}
        public double? ChangeToday {get; set;}

        // public double ExchangeRate {get; set;} = 1;
    }

    public class Holding : BaseStock
    {
        public Holding(TransactionEntity transaction)
        {
            Symbol = transaction.Symbol;
            Name = transaction.Name;
            Currency = transaction.Currency;
        }

        public double BuyPrice {get; set;}
        // public double BuyExchangeRate {get; set;}
        public int AmountOwned {get; set;}
        public DateTimeOffset BuyDate {get; set;}
        public DateTimeOffset? SoldDate {get; set;}
        public double? TotalChange {get => Price / BuyPrice;}
        public double? TotalValue {get => Price * AmountOwned;}
        public double? BuyValue {get => BuyPrice * AmountOwned;}
        public double? ReturnValue {get => TotalValue - BuyValue;}
        public double? ReturnPercentage {get => TotalValue / BuyValue;}
    }
}