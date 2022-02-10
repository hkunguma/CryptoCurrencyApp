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

namespace CryptoCurrencyApi.Tests
{
    [Collection("ExchangeRate collection")]
    public class ExchangeRateUnitTest
    {
        private readonly ExchangeRateFixture exchangeRateFixture;

        public ExchangeRateUnitTest(ExchangeRateFixture exchangeRateFixture)
        {
            this.exchangeRateFixture = exchangeRateFixture;
        }

        [Fact]
        public async Task Get_ExchangeRate_By_Currency_Successfully_Returns_Rates()
        {
            // Arrange
            var controller = new ExchangeRateController(
               exchangeRateFixture.MockMemoryCache, exchangeRateFixture.MockCoinbaseConfig,
               exchangeRateFixture.MockExchangeRate.Object, exchangeRateFixture.MockGenericRepo.Object);

            // Act
            var result = await controller.Get(exchangeRateFixture.Currency);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ExchangeRateResult>(okResult.Value);
            Assert.Equal("BTC", returnValue.Data.Currency);
            Assert.True(returnValue.Data.Rates.ContainsKey("USD"));
        }

        [Fact]
        public async Task Get_ExchangeRate_By_InvalidCurrency_Fails_Returns_BadRequest()
        {
            // Arrange
            var controller = new ExchangeRateController(
               exchangeRateFixture.MockMemoryCache, exchangeRateFixture.MockCoinbaseConfig,
               exchangeRateFixture.MockExchangeRate.Object, exchangeRateFixture.MockGenericRepo.Object);

            // Act
            var result = await controller.Get((AllowedCurrencyEnum)3);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Currency 3 is not valid", badRequestObjectResult.Value);
        }

    }
}
