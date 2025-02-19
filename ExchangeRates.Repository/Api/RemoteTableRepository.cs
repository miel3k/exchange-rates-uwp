﻿using ExchangeRates.Model;
using ExchangeRates.Repository.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.Repository.Remote
{
    public class RemoteTableRepository : ITableRepository
    {
        private readonly HttpHelper _http;

        public RemoteTableRepository(string baseUrl) => _http = new HttpHelper(baseUrl);

        public async Task<IEnumerable<Table>> GetAsync(DateTime date) =>
            await _http.GetAsync<IEnumerable<Table>>($"exchangeRates/tables/a/{date.ToString(Constants.DatePattern)}");
    }
}
