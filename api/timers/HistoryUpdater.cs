using System.Threading.Tasks;
using api.BLL;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace api.timers
{
    public class HistoryUpdater
    {
        private readonly PortfolioLogic _logic;

        public HistoryUpdater(PortfolioLogic logic)
        {
            this._logic = logic;
        }

        [FunctionName("TimerTriggerCSharp")]
        public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
        {
            await _logic.UpdateCurrencyExchangeRates();
        }
        
    }
}