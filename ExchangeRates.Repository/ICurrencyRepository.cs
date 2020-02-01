using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRates.Model
{
    public interface ICurrencyRepository
    {
        Task<CurrencyTable> GetAsync(
            string code,
            DateTime startDate,
            DateTime endDate,
            string filepath,
            IProgress<float> progress,
            CancellationToken cancellationToken
        );
    }
}
