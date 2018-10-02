using CarsApp.Model;
using CarsApp.Model.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsWebAPI.Tests.Mocks
{
    public class InMemoryCarsService : ICarsService
    {
        public Dictionary<ObjectId, Car> Cars { get; } = new Dictionary<ObjectId, Car>();

        public Task<Car> Get(ObjectId id)
        {
            return Task.FromResult(Cars.TryGetValue(id, out var car) ? car : null);
        }

        public Task<IEnumerable<Car>> Get()
        {
            return Task.FromResult(Cars.Values.AsEnumerable());
        }

        public Task Remove(ObjectId id)
        {
            Cars.Remove(id);
            return Task.CompletedTask;
        }

        public Task Remove(ObjectId[] ids)
        {
            foreach (var id in ids)
            {
                Cars.Remove(id);
            }
            return Task.CompletedTask;
        }

        public Task<Car> Insert(Car car)
        {
            Cars[car.Id] = car;
            return Task.FromResult(car);
        }

        public Task Update<TValue>(ObjectId id, Func<Car, bool> filter, Func<Car, TValue> updateField, TValue value)
        {
           var field = Cars.Values.Where(filter).Select(updateField);
            throw new Exception();
        }
    }
}
