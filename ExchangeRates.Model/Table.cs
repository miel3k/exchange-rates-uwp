using System;
using System.Collections.Generic;

namespace ExchangeRates.Model
{
    public class Table
    {
        public string Type { get; set; }
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public List<Rate> Rates { get; set; } = new List<Rate>();
        public override string ToString() => No.ToString();
    }
}
