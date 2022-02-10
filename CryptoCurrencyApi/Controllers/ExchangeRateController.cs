using CryptoCurrencyApi.DAL.EF;
using CryptoCurrencyApi.DAL.EF.Repository;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Configuration;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Interfaces;
using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IExchangeRate _exchangeRate;
        private readonly IGenericRepository<Coinbase> _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly CoinbaseConfig _coinbaseConfig;

        public ExchangeRateController(IMemoryCache memoryCache, CoinbaseConfig coinbaseConfig, IExchangeRate exchangeRate, 
            IGenericRepository<Coinbase> repository)
        {
            _memoryCache = memoryCache;
            _coinbaseConfig = coinbaseConfig;
            _exchangeRate = exchangeRate;
            _repository = repository;
        }

        // GET: api/<ExchangeRateController>
        [HttpGet]
        public async Task<IActionResult> Get(AllowedCurrencyEnum currency)
        {
            if (!Enum.IsDefined(typeof(AllowedCurrencyEnum), currency))
                return BadRequest($"Currency {currency.ToString()} is not valid");

            var result = await GetExchangeRate(currency.ToString());

            if (result == null)
                return NotFound(currency);

            return Ok(result);
        }

        private async Task<ExchangeRateResult> GetExchangeRate(string currency)
        {
            var cacheKey = "exchangeRate";

            if (!_memoryCache.TryGetValue(cacheKey, out ExchangeRateResult exchangeRateResult))
            {
                exchangeRateResult = await _exchangeRate.GetExchangeRate(currency);

                int.TryParse(_coinbaseConfig.CacheDuration, out int cacheDuration);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(cacheDuration),
                    SlidingExpiration = TimeSpan.FromMinutes(cacheDuration),
                    Priority = CacheItemPriority.High
                };

                _memoryCache.Set(cacheKey, exchangeRateResult, cacheOptions);

                //create entry in DB of request and response
                var coinbase = new Coinbase()
                {
                    UserRequest = $"{_coinbaseConfig.CoinbaseBaseUrl}{_coinbaseConfig.ExchangeRateEndpoint}?currency={currency}",
                    Response = JsonConvert.SerializeObject(exchangeRateResult)
                };

                _repository.Insert(coinbase);
                _repository.Save();
            }

            return exchangeRateResult;            
        }

    }
}
