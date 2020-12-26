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
        string ExchangeRate {get; set;}
        DateTimeOffset Date {get; set;}
    }

    public class NordeaExportFormat : INordeaExportFormat
    {
        [Name("Ordre nr.")]
        public string OrderNumber {get; set;}
        
        [Name("Navn")]
        public string Name {get; set;}
        
        [Name("Køb / Salg")]
        public string OrderType {get; set;}
        
        [Name("Stk / Nom")]
        public int Pieces {get; set;}
        
        [Name("Kurs")]
        public double Price {get; set;}
        
        [Name("Afregningsbeløb")]
        public double TotalAmount {get; set;}
        
        [Name("Konto")]
        public string Account {get; set;}

        public double Fee {get; set;}
        public string Symbol {get; set;}
        public string Currency {get; set;}
        public string ExchangeRate {get; set;}
        public DateTimeOffset Date {get; set;}
    }
}