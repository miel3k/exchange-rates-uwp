using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRates.Model;
using Windows.Storage;

namespace ExchangeRates.Repository.Api
{
    public class RemoteCurrencyRepository : ICurrencyRepository
    {
        private readonly HttpHelper _http;

        public RemoteCurrencyRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public async Task<CurrencyTable> GetAsync(string code, DateTime startDate, DateTime endDate, string filepath, IProgress<float> progress)
        {
            CancellationToken cancellationToken = new CancellationToken();
            using (var file = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                return await _http.GetAsyncWithProgress<CurrencyTable>($"exchangeRates/rates/a/{code}/{startDate.ToString(@"yyyy-MM-dd")}/{endDate.ToString(@"yyyy-MM-dd")}", file, progress, cancellationToken);
            }
        }
    }
}
