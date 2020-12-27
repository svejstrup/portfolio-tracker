using System;
using CsvHelper.Configuration.Attributes;

namespace api.models
{
    public interface INordeaExportFormat
    {
        string OrderNumber {get; set;}
        string Name {get; set;}
        string OrderType {get; set;}
        int Pieces {get; set;}
        double Price {get; set;}
        double TotalAmount {get; set;}
        string Account {get; set;}
        double Fee {get; set;}
        string Symbol {get; set;}
        string Currency {get; set;}
        double ExchangeRate {get; set;}
        DateTimeOffset Date {get; set;}
    }

    public class NordeaExportFormat : INordeaExportFormat
    {
        [Name("Ordrenr.")]
        public string OrderNumber {get; set;}
        
        [Name("Navn")]
        public string Name {get; set;}
        
        [Name("Køb / Salg")]
        public string OrderType {get; set;}
        
        [Name("Stk. / Nom.")]
        public int Pieces {get; set;}
        
        [Name("Kurs")]
        public double Price {get; set;}
        
        [Name("Afregningsbeløb")]
        public double TotalAmount {get; set;}
        
        [Name("Konto")]
        public string Account {get; set;}

        [Name("Fee")]
        public double Fee {get; set;}
        [Name("Symbol")]
        public string Symbol {get; set;}
        [Name("Currency")]
        public string Currency {get; set;}
        [Name("ExchangeRate")]
        public double ExchangeRate {get; set;}
        [Name("Date")]
        public DateTimeOffset Date {get; set;}
    }
}