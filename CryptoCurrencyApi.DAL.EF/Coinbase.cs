using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.DAL.EF
{
    public class Coinbase
    {
        public int Id { get; set; }
        public string UserRequest { get; set; }
        public string Response { get; set; }
    }
}
