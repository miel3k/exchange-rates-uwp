using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRates.Model
{
    public class Table
    {
        public string Type { get; set; }

        public string No { get; set; }

        public DateTime EffectiveDate { get; set; }

        public List<Rate> rates { get; set; } = new List<Rate>();

        public override string ToString() => No.ToString();
    }
}
