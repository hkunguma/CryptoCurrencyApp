using System;
using Xunit;
using CryptoCurrencyApi;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Configuration;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Interfaces;
using CryptoCurrencyApi.DAL.EF.Repository;
using CryptoCurrencyApi.DAL.EF;
using CryptoCurrencyApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoCurrencyApi.Tests
{
    public class ExchangeRateFixture : IDisposable
    {
        public ExchangeRateFixture()
        {
            // Setup
            Currency = AllowedCurrencyEnum.BTC;

            // set concrete implementation of memory cache
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            MockMemoryCache = serviceProvider.GetService<IMemoryCache>();
            //ExchangeRateResult exchangeRateResult;
            //MockMemoryCache.Setup(cache => cache.TryGetValue("exchangeRate", out exchangeRateResult)).Returns(false);

            MockCoinbaseConfig = new CoinbaseConfig
            {
                CoinbaseBaseUrl = "https://api.coinbase.com/v2/",
                ExchangeRateEndpoint = "exchange-rates",
                CacheDuration = "1"
            };

            MockExchangeRate = new Mock<IExchangeRate>();
            MockExchangeRate.Setup(exc => exc.GetExchangeRate(Currency.ToString())).ReturnsAsync(GetExchangeRates());

            MockGenericRepo = new Mock<IGenericRepository<Coinbase>>();
        }

        public AllowedCurrencyEnum Currency { get; private set; }
        public IMemoryCache MockMemoryCache { get; private set; }
        public CoinbaseConfig MockCoinbaseConfig { get; private set; }
        public Mock<IExchangeRate> MockExchangeRate { get; private set; }
        public Mock<IGenericRepository<Coinbase>> MockGenericRepo { get; private set; }

        public void Dispose()
        {
            
        }

        private ExchangeRateResult GetExchangeRates()
        {
            var exchangeRate = new ExchangeRateResult
            {
                Data = new ExchangeRateData
                {
                    Currency = "BTC",
                    Rates = new Dictionary<string, string> {
                        {"AED","159427.7085765"},
                        {"AFN","4126695.185676875"},
                        {"ALL","4610962.655906775"},
                        {"AMD","20823215.7881125"},
                        {"ANG","78227.621115175"},
                        {"AOA","22662614.353925"},
                        { "USD","43405.075"}
                    }
                }
            };
            return exchangeRate;
        }
    }
}
