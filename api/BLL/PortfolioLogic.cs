using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DAL;
using api.models;
using api.models.entities;
using api.SAL;
using api.util;
using Microsoft.Extensions.Logging;

namespace api.BLL
{
    public class PortfolioLogic
    {
        private readonly YahooFinanceClient _yahooFinanceClient;
        private readonly TransactionDataManager _transactionDataManager;
        private readonly ILogger<PortfolioLogic> _logger;

        public PortfolioLogic(YahooFinanceClient yahooFinanceClient, TransactionDataManager transactionDataManager, ILogger<PortfolioLogic> logger)
        {
            _yahooFinanceClient = yahooFinanceClient;
            _transactionDataManager = transactionDataManager;
            _logger = logger;
        }

        public async Task<Portfolio> GetPortfolio()
        {
            var transactions = await _transactionDataManager.GetAll();
            var portfolio = await GetPortfolioFromTransactions(transactions);

            return portfolio;
        }

        private async Task<Portfolio> GetPortfolioFromTransactions(List<TransactionEntity> transactions)
        {
            var portfolio = new Portfolio();
            
            foreach (var group in transactions.GroupBy(t => t.Symbol))
            {
                var (currentHolding, previousHoldings) = GetHoldingsSingleStock(group.ToList());
                
                if (currentHolding != null)
                    portfolio.CurrentHoldings.Add(currentHolding);
                
                portfolio.PreviousHoldings.AddRange(previousHoldings);
            }

            portfolio.CurrentHoldings = await UpdateCurrentPriceOfHoldings(portfolio.CurrentHoldings);

            return portfolio;
        }

        private (Holding currentHolding, List<Holding> previousHoldings) GetHoldingsSingleStock(List<TransactionEntity> transactionEntities)
        {
            Holding currentHolding = null; 

            var previousHoldings = new List<Holding>();

            var sharesOwned = 0;
            var costOfOwned = 0.0;
            var buyDate = DateTimeOffset.MinValue;

            foreach(var t in transactionEntities.OrderBy(t => t.Date))
            {
                if (!Enum.TryParse(t.OrderType, out TransactionType transactionType))
                {
                    _logger.LogWarning("Unable to parse transaction type {transactionType}", transactionType);
                    continue;
                }

                if (buyDate == DateTimeOffset.MinValue)
                    buyDate = t.Date;
               
                switch(transactionType)
                {
                    case TransactionType.Køb:
                        sharesOwned += t.Pieces;
                        costOfOwned += t.Pieces * t.Price * t.ExchangeRate + t.Fee;
                        break;
                    case TransactionType.Salg:
                        costOfOwned += t.Fee;
                        var pricePerShare = costOfOwned / sharesOwned; 
                        previousHoldings.Add(new Holding(t)
                        {
                            AmountOwned = t.Pieces,
                            BuyDate = buyDate,
                            BuyPrice = pricePerShare,
                            Price = t.Price * t.ExchangeRate,
                            SoldDate = t.Date
                        });

                        sharesOwned -= t.Pieces;
                        costOfOwned -= t.Pieces * pricePerShare;
                        break;
                }
            }

            if (sharesOwned > 0)
            {
                currentHolding = new Holding(transactionEntities.First())
                {
                    AmountOwned = sharesOwned,
                    BuyDate = buyDate,
                    BuyPrice = costOfOwned / sharesOwned
                };
            }

            return (currentHolding, previousHoldings);
        }

        private async Task<List<Holding>> UpdateCurrentPriceOfHoldings(List<Holding> holdings)
        {
            var symbols = holdings.Select(h => h.Symbol).ToList();
            var currentPrices = await _yahooFinanceClient.GetCurrentPrices(symbols);

            holdings = holdings
                .Select(h => 
                {
                    if (currentPrices.TryGetValue(h.Symbol, out var currentStockPrice))
                    {
                        h.Price = currentStockPrice.Price;
                        h.ChangeToday = currentStockPrice.ChangeToday;
                    }
                    else 
                    {
                        _logger.LogWarning("Symbol: {symbol} not found", h.Symbol);
                    }

                    return h;
                }).ToList();

            return holdings;
        }
    }
}