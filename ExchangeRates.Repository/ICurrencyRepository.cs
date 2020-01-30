using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Model
{
    public interface ICurrencyRepository
    {
        Task<CurrencyTable> GetAsync(String code, DateTime startDate, DateTime endDate, string filepath, IProgress<float> progress);
    }
}
