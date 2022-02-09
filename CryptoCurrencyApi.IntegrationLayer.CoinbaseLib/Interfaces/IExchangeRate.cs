using CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Interfaces
{
    public interface IExchangeRate
    {
        Task<ExchangeRateResult> GetExchangeRate(string currency);
    }
}
