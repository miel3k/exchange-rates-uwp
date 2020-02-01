using System.Collections.Generic;

namespace ExchangeRates.Model
{
    public class CurrencyTable
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<CurrencyRate> Rates { get; set; } = new List<CurrencyRate>();
        public override string ToString() => Table.ToString();
    }
}
