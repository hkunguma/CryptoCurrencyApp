using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCurrencyApi.DAL.EF.Repository
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        void Insert(TEntity entity);
        void Save();
    }
}
