using CryptoCurrencyApi.CrossCuttingLayer.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.CrossCuttingLayer.Helpers
{
    public class HttpClientHelper<TEntity> : IHttpClient<TEntity>
    {
        private readonly string _baseAddress;

        private readonly string _authScheme;
        private readonly string _authToken;

        //private HttpClient _httpClient;

        public HttpClientHelper(string baseAddress, string authScheme = null, string authToken = null)
        {
            _baseAddress = baseAddress;

            //_httpClient = Client();
        }

        public async Task<TEntity> Get(string requestUri)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            try
            {
                using (HttpClient client = Client())
                {                    
                    httpResponse = await client.GetAsync(requestUri);

                    if (httpResponse == null)
                        return default(TEntity);

                    using (HttpContent content = httpResponse.Content)
                    {
                        string result = await content.ReadAsStringAsync();

                        var response = JsonConvert.DeserializeObject<TEntity>(result);

                        return response;
                    }
                }
            }
            catch (HttpRequestException)
            {

            }
            catch (Exception)
            {
            }

            return default(TEntity);
        }

        //public void Dispose()
        //{
        //    _httpClient.Dispose();
        //}

        private HttpClient Client()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(_authScheme))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authScheme, _authToken);

            return httpClient;
        }

    }
}
