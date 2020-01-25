using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Repository.Api
{
    internal class HttpHelper
    {
        private readonly string _baseUrl;

        public HttpHelper(string baseUrl) => _baseUrl = baseUrl;

        public async Task<TResult> GetAsync<TResult>(string controller)
        {
            using (var client = BaseClient())
            {
                var response = await client.GetAsync(controller);
                string json = await response.Content.ReadAsStringAsync();
                TResult obj = JsonConvert.DeserializeObject<TResult>(json);
                return obj;
            }
        }

        private HttpClient BaseClient() => new HttpClient { BaseAddress = new Uri(_baseUrl) };

        private class JsonStringContent : StringContent
        {
            public JsonStringContent(object obj)
                : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
    }
}
