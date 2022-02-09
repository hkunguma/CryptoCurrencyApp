using CryptoCurrencyApi.CrossCuttingLayer.Contracts;
using CryptoCurrencyApi.CrossCuttingLayer.Helpers;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Configuration;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Interfaces;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.IntegrationLayer.CoinbaseLib
{
    public class ExchangeRate : IExchangeRate
    {
        private readonly CoinbaseConfig _coinbaseConfig;        

        public ExchangeRate(CoinbaseConfig coinbaseConfig)
        {
            _coinbaseConfig = coinbaseConfig;
        }
        public async Task<ExchangeRateResult> GetExchangeRate(string currency)
        {
            IHttpClient<ExchangeRateResult> _httpClient = new HttpClientHelper<ExchangeRateResult>(_coinbaseConfig.CoinbaseBaseUrl);

            StringBuilder requestUri = new StringBuilder(_coinbaseConfig.ExchangeRateEndpoint);
            requestUri.Append($"?currency={currency}");

            return await _httpClient.Get(requestUri.ToString());
        }
    }
}
