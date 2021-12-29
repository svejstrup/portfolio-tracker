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
        private readonly CurrencyClient _currencyClient;
        private readonly CurrencyDataManager _currencyDataManager;

        public PortfolioLogic(YahooFinanceClient yahooFinanceClient, TransactionDataManager transactionDataManager, ILogger<PortfolioLogic> logger, CurrencyClient currencyClient, CurrencyDataManager currencyDataManager)
        {
            _yahooFinanceClient = yahooFinanceClient;
            _transactionDataManager = transactionDataManager;
            _logger = logger;
            _currencyClient = currencyClient;
            _currencyDataManager = currencyDataManager;
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
            
            foreach (var group in transactions.GroupBy(t => t.IsinCode.Trim().ToLowerInvariant()))
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
                    case TransactionType.Udbyttebevis:
                        sharesOwned += t.Pieces.Value;
                        costOfOwned += t.Pieces.Value * t.Price * t.ExchangeRate + t.Fee;
                        break;
                    case TransactionType.Salg:
                        var pricePerShare = sharesOwned == 0 ? 0 : costOfOwned / sharesOwned; // Calculate average buy price per share of currently owned shares

                        previousHoldings.Add(new Holding(t)
                        {
                            AmountOwned = t.Pieces.Value,
                            BuyDate = buyDate,
                            BuyPrice = pricePerShare,
                            Price = t.Price * t.ExchangeRate - (t.Fee / t.Pieces),
                            SoldDate = t.Date
                        });

                        sharesOwned -= t.Pieces.Value;
                        costOfOwned -= t.Pieces.Value * pricePerShare;
                        break;
                    case TransactionType.Udbytte:
                        previousHoldings.Add(new Holding(t)
                        {
                            AmountOwned = 1,
                            BuyDate = buyDate,
                            BuyPrice = 0,
                            Price = t.TotalAmount,
                            SoldDate = t.Date
                        });
                        break;
                }

                // Reset buy date if all shares are sold
                if (sharesOwned == 0)
                    buyDate = DateTimeOffset.MinValue;
            }

            if (sharesOwned > 0 && !transactionEntities.All(te => string.IsNullOrWhiteSpace(te.Symbol)))
            {
                currentHolding = new Holding(transactionEntities.FirstOrDefault(te => !string.IsNullOrWhiteSpace(te.Symbol)))
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

            var distinctCurrencies = holdings.Select(h => h.Currency).ToHashSet();

            var exchangeRateMap = await _currencyDataManager.GetLatest(distinctCurrencies);

            holdings = holdings
                .Select(h => 
                {
                    if (currentPrices.TryGetValue(h.Symbol, out var currentStockPrice))
                    {
                        h.Price = currentStockPrice.Price * exchangeRateMap[h.Currency];
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