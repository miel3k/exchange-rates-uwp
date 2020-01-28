using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRates.Model
{
    public class CurrencyTable
    {
        public string Table { get; set; }

        public string Currency { get; set; }

        public string Code { get; set; }

        public List<CurrencyRate> CurrencyRates { get; set; } = new List<CurrencyRate>();

        public override string ToString() => Table.ToString();
    }
}
