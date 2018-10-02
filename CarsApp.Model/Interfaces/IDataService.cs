using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsApp.Model.Interfaces
{
    public interface IDataService<TEntity, TIdentifier>
    {
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> Get(TIdentifier id);
        Task<TEntity> Insert(TEntity car);
        Task Update<TValue>(TIdentifier id, Func<Car, bool> filter, Func<Car, TValue> updateField, TValue value);
        Task Remove(TIdentifier id);
        Task Remove(TIdentifier[] ids);
    }
}
