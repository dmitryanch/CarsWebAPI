using CarApp.Model.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo
{
    /// <summary>
    /// This class provides async CRUD API for mongo collections, It wraps Mongodb.Driver
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    /// <typeparam name="TIdentifier">Type of Entity Identifier</typeparam>
    public class CollectionAccessor<TEntity, TIdentifier> where TEntity : IEntity<TIdentifier>
    {
        IMongoCollection<TEntity> _collection;

        public CollectionAccessor(IMongoCollection<TEntity> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Get All Entities from collection Async
        /// </summary>
        /// <returns>Collection of entities wrapped into Task</returns>
        public async Task<IEnumerable<TEntity>> Get()
        {
            var docs = await _collection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await docs.ToListAsync();
        }

        /// <summary>
        /// Get specified Entity by Id
        /// </summary>
        /// <param name="id">The Identifier</param>
        /// <returns></returns>
        public async Task<TEntity> Get(TIdentifier id)
        {
            var docs = await _collection.FindAsync(x => x.Id.Equals(id));
            return await docs.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Saves Entity with upsert option Async
        /// </summary>
        /// <param name="obj">Specified Entity</param>
        /// <param name="isUpsert">Is need to create entity if that doesn't exist</param>
        /// <returns>Saved Entity</returns>
        public async Task<TEntity> Save(TEntity obj, bool isUpsert)
        {
            await _collection.ReplaceOneAsync(x => x.Id.Equals(obj.Id), obj, new UpdateOptions
            {
                IsUpsert = isUpsert
            });
            return obj;
        }

        /// <summary>
        /// Removes specified Entity Async
        /// </summary>
        /// <param name="id">Entity Identifier</param>
        /// <returns>Raw Task</returns>
        public async Task Remove(TIdentifier id)
        {
            await _collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Removes specified Entities Async
        /// </summary>
        /// <param name="ids">Enitity Identifiers</param>
        /// <returns>Raw Task</returns>
        public async Task Remove(TIdentifier[] ids)
        {
            await _collection.DeleteManyAsync(x => ids.Contains(x.Id));
        }
    }
}
