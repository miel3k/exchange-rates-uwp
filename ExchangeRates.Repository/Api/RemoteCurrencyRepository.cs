using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExchangeRates.Model;

namespace ExchangeRates.Repository.Api
{
    public class RemoteCurrencyRepository : ICurrencyRepository
    {
        private readonly HttpHelper _http;

        public RemoteCurrencyRepository(string baseUrl)
        {
            _http = new HttpHelper(baseUrl);
        }

        public async Task<CurrencyTable> GetAsync(string code, DateTime startDate, DateTime endDate) =>
            await _http.GetAsync<CurrencyTable>($"exchangeRates/rates/a/{code}/{startDate.ToString(@"yyyy-MM-dd")}/{endDate.ToString(@"yyyy-MM-dd")}");
    }
}
