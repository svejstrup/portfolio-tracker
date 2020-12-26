using System;
using System.Threading.Tasks;
using api.models;

namespace api.DAL
{
    public class TransactionDataManager
    {
        private readonly TableStorageDataManager _tableStorageDataManager;

        public TransactionDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager("Transactions");
        }

        public async Task<Portfolio> GetPortfolioFromTransactions()
        {   await Task.Delay(1);

            return new Portfolio();
        }
    }
}