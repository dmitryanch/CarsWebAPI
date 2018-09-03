using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarApp.Model.Interfaces
{
    public interface IDataService<TEntity, TIdentifier>
    {
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> Get(TIdentifier id);
        Task<TEntity> Save(TEntity car, bool IsUpsert);
        Task Remove(TIdentifier id);
        Task Remove(TIdentifier[] ids);
    }
}
