using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.DAL.EF
{
    public static class DbInitializer
    {
        public static void Initialize(CoinbaseContext context)
        {
            context.Database.EnsureCreated();

            ////Look for any coinbase
            //if (context.Coinbases.Any())
            //{
            //    return; // DB has been seeded
            //}
        }
    }
}
