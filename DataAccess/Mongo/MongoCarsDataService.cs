using CarApp.Model;
using CarApp.Model.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo
{
    /// <summary>
    /// Implements 
    /// </summary>
    public sealed class MongoCarsDataService : ICarsService
    {
        #region Private Fields
        private IMongoClient _client;
        private IMongoDatabase _db;
        private CollectionAccessor<Car, int> _cars;
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
        private CollectionAccessor<Car, int> Cars
        {
            get => _cars ?? (_cars = new CollectionAccessor<Car, int>(_db.GetCollection<Car>("Cars")));
        }
        #endregion

        #region Public API
        public async Task<Car> Get(int id)
        {
            return await Cars.Get(id);
        }

        public async Task<IEnumerable<Car>> Get()
        {
            return await Cars.Get();
        }

        public async Task Remove(int id)
        {
            await Cars.Remove(id);
        }

        public async Task Remove(int[] ids)
        {
            await Cars.Remove(ids);
        }

        public async Task<Car> Save(Car car, bool isUpsert)
        {
            return await Cars.Save(car, isUpsert);
        }
        #endregion
    }
}
