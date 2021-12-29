using System;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace api.models
{
    public interface INordeaExportFormat
    {
        string OrderNumber {get; set;}
        string Name {get; set;}
        string OrderType {get; set;}
        int? Pieces {get; set;}
        double Price {get; set;}
        string Account {get; set;}
        double Fee {get; set;}
        string Symbol {get; set;}
        string Currency {get; set;}
        double ExchangeRate {get; set;}
        DateTimeOffset Date {get; set;}
        string StockExchange {get; set;}
        double TotalAmount {get; set;}
        string IsinCode {get; set;}
    }

    public class NordeaExportFormat : INordeaExportFormat
    {
        [Name("Hovedordrenr.")]
        public string OrderNumber {get; set;}
        
        [Name("Navn")]
        public string Name {get; set;}
        
        [Name("Køb / Salg")]
        public string OrderType {get; set;}
        
        [Name("Stk. / Nom.")]
        public int? Pieces {get; set;}
        
        [Name("Kurs")]
        public double Price {get; set;}
        
        [Name("Depot")]
        public string Account {get; set;}

        [Name("Kurtage")]
        public double Fee {get; set;}
        [Name("Symbol")]
        public string Symbol {get; set;}
        [Name("Valuta")]
        public string Currency {get; set;}
        [Name("Valutakurs")]
        public double ExchangeRate {get; set;}
        [Name("Handelstidspunkt")]
        public DateTimeOffset Date {get; set;}
        [Name("Børs")]
        public string StockExchange { get; set; }
        [Name("Afregningsbeløb")]
        public double TotalAmount {get; set;}
        [Name("ISIN kode")]
        public string IsinCode {get; set;}

    }

    public class NordeaExportFormatMap : ClassMap<NordeaExportFormat> 
    {
        public NordeaExportFormatMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Date).TypeConverterOption.Format("dd-MM-yyyy");
            Map(m => m.TotalAmount).ConvertUsing(row => 
            {
                if (double.TryParse(row.GetField("Afregningsbeløb", 1), NumberStyles.Any, CultureInfo.InvariantCulture, out var amount))
                    return amount;
                
                return 0;
            });
        }
    }
}