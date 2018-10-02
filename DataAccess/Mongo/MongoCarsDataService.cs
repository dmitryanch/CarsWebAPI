using CarsApp.Model;
using CarsApp.Model.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsApp.MongoORM
{
    /// <summary>
    /// Implements 
    /// </summary>
    public sealed class MongoCarsDataService : ICarsService
    {
        #region Private Fields
        private IMongoClient _client;
        private IMongoDatabase _db;
        private CollectionAccessor<Car, ObjectId> _cars;
        #endregion

        public MongoCarsDataService(string connectionString, string dbName)
        {
            if (connectionString == null)
            {
                throw new NullReferenceException($"{nameof(connectionString)}");
            }
            if (dbName == null)
            {
                throw new NullReferenceException($"{nameof(dbName)}");
            }
            _client = new MongoClient(connectionString);
            _db = _client.GetDatabase(dbName);
        }

        #region Private Properties and Methods
        private CollectionAccessor<Car, ObjectId> Cars
        {
            get => _cars ?? (_cars = new CollectionAccessor<Car, ObjectId>(_db.GetCollection<Car>("Cars")));
        }
        #endregion

        #region Public API
        public async Task<Car> Get(ObjectId id)
        {
            return await Cars.Get(id);
        }

        public async Task<IEnumerable<Car>> Get()
        {
            return await Cars.Get();
        }

        public async Task Remove(ObjectId id)
        {
            await Cars.Remove(id);
        }

        public async Task Remove(ObjectId[] ids)
        {
            await Cars.Remove(ids);
        }

        public async Task<Car> Insert(Car car)
        {
            return await Cars.Insert(car);
        }

        public async Task Update<TValue>(ObjectId id, Func<Car, bool> filter, Func<Car, TValue> updateField, TValue value)
        {
            await Cars.Update(id, filter, updateField, value);
        }
        #endregion
    }
}
