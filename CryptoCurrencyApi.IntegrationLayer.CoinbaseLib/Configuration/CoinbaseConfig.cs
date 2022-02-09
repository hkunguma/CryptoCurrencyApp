using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Configuration
{
    public class CoinbaseConfig
    {
        public string AuthUrl { get; set; }
        public string AccessTokenUrl { get; set; }
        public string AuthScheme { get; set; }
        public string ClientId { get; set; }
        public string CoinbaseBaseUrl { get; set; }
        public string ExchangeRateEndpoint { get; set; }
        public string CacheDuration { get; set; }
    }
}
