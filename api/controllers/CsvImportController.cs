using System.Threading.Tasks;
using api.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace api.controllers
{
    public class CsvImportController
    {
        private const string ControllerName = "CsvImport/";

        private readonly ImportLogic _importLogic;

        public CsvImportController(ImportLogic importLogic)
        {
            _importLogic = importLogic;
        }

        [FunctionName(nameof(ImportTransactions))]
        public async Task<IActionResult> ImportTransactions(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = ControllerName + nameof(ImportTransactions))] HttpRequest req,
            ILogger log)
        {
            var files = req?.Form?.Files;

            if (files == null || files.Count == 0)
                return new NotFoundResult();

            await _importLogic.ImportTransactions(files[0]);
            return new OkResult();
        }
    }
}