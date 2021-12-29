using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.DAL;
using api.models;
using api.models.entities;
using api.SAL;
using api.util;
using CsvHelper;
using Microsoft.AspNetCore.Http;

namespace api.BLL
{
    public class ImportLogic
    {
        private readonly TransactionDataManager _transactionsDataManager;
        private readonly YahooFinanceClient _yahooFinanceClient;

        public ImportLogic(TransactionDataManager transactionsDataManager, YahooFinanceClient yahooFinanceClient)
        {
            _transactionsDataManager = transactionsDataManager;
            _yahooFinanceClient = yahooFinanceClient;
        }

        public async Task ImportTransactions(IFormFile file)
        {
            var dbTransactions = await _transactionsDataManager.GetAll();

            var newTransactions = ParseCsv(file)
                .Select(nef => new TransactionEntity(nef))
                .ToList();

            var dividends = newTransactions
                .Where(t => Enum.Parse<TransactionType>(t.OrderType) == TransactionType.Udbyttebevis)
                .ToList();

            // Fetch and set price of stocks bought with udyttebevis as this is not specified in the transaction
            foreach(var d in dividends)
            {
                var t = newTransactions
                    .FirstOrDefault(t => t.IsinCode == d.IsinCode && !string.IsNullOrWhiteSpace(t.Symbol));

                if (t == null)
                    continue;

                d.Symbol = t.Symbol;
                d.StockExchange = t.StockExchange;

                var price = await _yahooFinanceClient.GetHistoricPrice(SymbolTranslation.TranslateSymbol(d), d.Date);
                d.Price = price;
                d.TotalAmount = price * d.Pieces.Value;
            }

            await _transactionsDataManager.InsertMany(newTransactions);

            // TODO - update history for new transactions
        }

        private List<NordeaExportFormat> ParseCsv(IFormFile file) 
        {
            using(var reader = new StreamReader(file.OpenReadStream()))
            using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.Delimiter = ",";
                csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;

                // csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower().Replace(" ", String.Empty).Replace(".",String.Empty).Replace("/", String.Empty);
                csv.Configuration.RegisterClassMap<NordeaExportFormatMap>();

                var records = csv.GetRecords<NordeaExportFormat>();

                return records.ToList();
            }
        }
    }
}