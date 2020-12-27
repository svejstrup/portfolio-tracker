using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.DAL;
using api.models;
using api.models.entities;
using CsvHelper;
using Microsoft.AspNetCore.Http;

namespace api.BLL
{
    public class ImportLogic
    {
        private readonly TransactionDataManager _transactionsDataManager;

        public ImportLogic(TransactionDataManager transactionsDataManager)
        {
            _transactionsDataManager = transactionsDataManager;
        }

        public async Task ImportTransactions(IFormFile file)
        {
            var dbTransactions = await _transactionsDataManager.GetAll();
            var existingOrderNumbers = dbTransactions.Select(t => t.OrderNumber).ToHashSet();

            var newTransactions = ParseCsv(file)
                .Select(nef => new TransactionEntity(nef))
                .Where(t => !existingOrderNumbers.Contains(t.OrderNumber))
                .ToList();

            await _transactionsDataManager.InsertMany(newTransactions);

            // TODO - update history for new transactions
        }

        private List<NordeaExportFormat> ParseCsv(IFormFile file) 
        {
            using(var reader = new StreamReader(file.OpenReadStream()))
            using(var csv = new CsvReader(reader, CultureInfo.GetCultureInfo("da-DK")))
            {
                // csv.Configuration.HasHeaderRecord = false;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower().Replace(" ", String.Empty).Replace(".",String.Empty).Replace("/", String.Empty);
                var records = csv.GetRecords<NordeaExportFormat>();

                return records.ToList();
            }
        }
    }
}