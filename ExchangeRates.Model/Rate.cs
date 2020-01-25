using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRates.Model
{
    public class Rate : IEquatable<Rate>
    {
        public string Currency { get; set; }

        public string Code { get; set; }

        public decimal Mid { get; set; }

        public override string ToString() => $"{Currency} {Code} {Mid}";

        public bool Equals(Rate other) =>
            Currency == other.Currency &&
            Code == other.Code &&
            Mid == other.Mid;
    }
}
