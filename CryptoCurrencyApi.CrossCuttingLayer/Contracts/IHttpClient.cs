using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.CrossCuttingLayer.Contracts
{
    public interface IHttpClient<TEntity>
    {
        Task<TEntity> Get(string requestUri);
    }
}
