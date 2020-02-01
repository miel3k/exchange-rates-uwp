using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRates.Model;

namespace ExchangeRates.Repository.Api
{
    public class RemoteCurrencyRepository : ICurrencyRepository
    {
        private readonly HttpHelper _http;
        public RemoteCurrencyRepository(string baseUrl) => _http = new HttpHelper(baseUrl);
        public async Task<CurrencyTable> GetAsync(
            string code,
            DateTime startDate,
            DateTime endDate,
            string filepath,
            IProgress<float> progress,
            CancellationToken cancellationToken
        )
        {
            using (var file = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                string route = $"exchangeRates/rates/a/{code}/{startDate.ToString(Constants.DatePattern)}/{endDate.ToString(Constants.DatePattern)}";
                return await _http.GetAsyncWithProgress<CurrencyTable>(route, file, progress, cancellationToken);
            }
        }
    }
}
