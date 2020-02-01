using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRates.Repository.Api
{
    internal class HttpHelper
    {
        private readonly string _baseUrl;
        public HttpHelper(string baseUrl) => _baseUrl = baseUrl;
        private HttpClient BaseClient() => new HttpClient { BaseAddress = new Uri(_baseUrl) };

        public async Task<TResult> GetAsync<TResult>(string controller)
        {
            using (var client = BaseClient())
            {
                var response = await client.GetAsync(controller);
                string json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TResult obj = JsonConvert.DeserializeObject<TResult>(json);
                    return obj;
                }
                else
                {
                    return default;
                }
            }
        }

        public async Task<TResult> GetAsyncWithProgress<TResult>(string controller, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            using (var client = BaseClient())
            {
                var response = await client.GetAsync(controller, HttpCompletionOption.ResponseHeadersRead);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var contentLength = response.Content.Headers.ContentLength;
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    if (progress == null || !contentLength.HasValue)
                    {
                        await stream.CopyToAsync(destination);
                        return default;
                    }
                    var relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
                    await stream.CopyToAsync(destination, Constants.DefaultBufferSize, relativeProgress, cancellationToken);
                    progress.Report(1);
                    TResult obj = JsonConvert.DeserializeObject<TResult>(json);
                    return obj;
                }
                else
                {
                    return default;
                }
            }
        }

        private class JsonStringContent : StringContent
        {
            public JsonStringContent(object obj)
                : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
            { }
        }
    }
}
