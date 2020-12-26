using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DAL;
using api.models;
using api.SAL;
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
            var portfolio = await _transactionDataManager.GetPortfolioFromTransactions();
            portfolio.CurrentHoldings = await UpdateCurrentPriceOfHoldings(portfolio.CurrentHoldings);

            return portfolio;
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