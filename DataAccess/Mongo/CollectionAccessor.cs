using CarsApp.Model.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.MongoORM
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
            var docs = await _collection.FindAsync(_ => true);
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
        /// Inserts Entity with upsert option Async
        /// </summary>
        /// <param name="obj">Specified Entity</param>
        /// <param name="isUpsert">Is need to create entity if that doesn't exist</param>
        /// <returns>Saved Entity</returns>
        public async Task<TEntity> Insert(TEntity obj)
        {
            await _collection.InsertOneAsync(obj);
            return obj;
        }

        /// <summary>
        /// Updates fields of the Entity
        /// </summary>
        /// <typeparam name="TValue">Type of the specified field</typeparam>
        /// <param name="filter">filter expression</param>
        /// <param name="updateField">update field expression</param>
        /// <param name="value">field value to update</param>
        /// <returns>Raw Task</returns>
        public async Task Update<TValue>(TIdentifier id, Func<TEntity, bool> filter, Func<TEntity, TValue> updateField, TValue value)
        {
            var filterDef = Builders<TEntity>.Filter.Eq(x => x.Id, id) & Builders<TEntity>.Filter.Eq(x => filter(x), true);
            var updateDef = Builders<TEntity>.Update.Set<TValue>(x => updateField(x), value);
            await _collection.FindOneAndUpdateAsync(x => filter(x), updateDef, new FindOneAndUpdateOptions<TEntity, TValue> { IsUpsert = true });
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
