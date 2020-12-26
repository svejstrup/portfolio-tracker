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
            
            // SAL
            builder.Services.AddSingleton<YahooFinanceClient>();
            
            // DAL
            builder.Services.AddSingleton<TransactionDataManager>();
        }
    }
}