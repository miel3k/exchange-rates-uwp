using System;

namespace ExchangeRates.Model
{
    public class Rate : IEquatable<Rate>
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public decimal Mid { get; set; }
        public string Uri { get => "https://www.countryflags.io/"+Code.Substring(0,2)+"/flat/64.png"; }
        public override string ToString() => $"{Currency} {Code} {Mid}";
        public bool Equals(Rate other) =>
            Currency == other.Currency &&
            Code == other.Code &&
            Mid == other.Mid;
    }
}
