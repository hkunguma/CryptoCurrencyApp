using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.IntegrationLayer.CoinbaseLib.Models
{
    public class ExchangeRateData
    {
        public string Currency { get; set; }
        public Dictionary<string, string> Rates { get; set; }
    }
}
