using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using api.BLL;

namespace api.controllers
{
    public class PortfolioController
    {
        private const string ControllerName = "portfolio/";
        private readonly PortfolioLogic _portfolioLogic;

        public PortfolioController(PortfolioLogic portfolioLogic)
        {
            _portfolioLogic = portfolioLogic;
        }

        [FunctionName(nameof(GetTableData))]
        public async Task<IActionResult> GetTableData(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = ControllerName + nameof(GetTableData))] HttpRequest req,
            ILogger log)
        {
            var portfolio = await _portfolioLogic.GetPortfolio();

            return new OkObjectResult(portfolio);
        }
    }
}
