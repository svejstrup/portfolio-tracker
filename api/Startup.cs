using api.BLL;
using api.DAL;
using api.SAL;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(api.Startup))]

namespace api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // BLL            
            builder.Services.AddSingleton<PortfolioLogic>();
            builder.Services.AddSingleton<ImportLogic>();
            
            // SAL
            builder.Services.AddSingleton<YahooFinanceClient>();
            builder.Services.AddHttpClient<CurrencyClient>();
            
            // DAL
            builder.Services.AddSingleton<TransactionDataManager>();
            builder.Services.AddSingleton<HistoryDataManager>();
            builder.Services.AddSingleton<CurrencyDataManager>();
        }
    }
}