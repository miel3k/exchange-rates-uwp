using System;

namespace ExchangeRates.Model
{
    public class CurrencyRate : IEquatable<CurrencyRate>
    {
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Mid { get; set; }
        public override string ToString() => $"{No} {EffectiveDate} {Mid}";

        public bool Equals(CurrencyRate other) =>
            No == other.No &&
            EffectiveDate == other.EffectiveDate &&
            Mid == other.Mid;
    }
}
