using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.models.entities;

namespace api.util
{
    public static class SymbolTranslation
    {
        private static readonly Dictionary<string, string> _symbolTranslation = new Dictionary<string, string>() {
            {"CARLb", "CARL-B.CO"},
            {"MAERSKa", "MAERSK-A.CO"},
            {"NOVOb", "NOVO-B.CO"},
            {"SAS", "SAS-DKK.CO"},
            {"CSP1", "CSPX.AS"},
            {"AKERBP", "AKRBP.OL"},
            {"VOWG_p", "VOW3.DE"},
            {"", ""},
        };

        public static string TranslateSymbol(TransactionEntity entity)
        {
            if (_symbolTranslation.ContainsKey(entity.Symbol))
                return _symbolTranslation[entity.Symbol];

             // Generally an identifier id appended to the symbol based on the stock exchange
            return entity.StockExchange switch
            {
                StockExchanges.OmxCopenhagen => entity.Symbol + ".CO",
                StockExchanges.OsloBors => entity.Symbol + ".OL",
                StockExchanges.Amsterdam => entity.Symbol + ".AS",
                StockExchanges.DeutcheBorse => entity.Symbol + ".DE",
                StockExchanges.Nasdaq => entity.Symbol,
                _ => entity.Symbol
            };
        }
    }
}