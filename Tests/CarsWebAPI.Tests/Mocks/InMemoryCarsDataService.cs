using CarApp.Model;
using CarApp.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsWebAPI.Tests.Mocks
{
    public class InMemoryCarsService : ICarsService
    {
        public Dictionary<int, Car> Cars { get; } = new Dictionary<int, Car>();

        public Task<Car> Get(int id)
        {
            return Task.FromResult(Cars.TryGetValue(id, out var car) ? car : null);
        }

        public Task<IEnumerable<Car>> Get()
        {
            return Task.FromResult(Cars.Values.AsEnumerable());
        }

        public Task Remove(int id)
        {
            Cars.Remove(id);
            return Task.CompletedTask;
        }

        public Task Remove(int[] ids)
        {
            foreach (var id in ids)
            {
                Cars.Remove(id);
            }
            return Task.CompletedTask;
        }

        public Task<Car> Save(Car car, bool isUpsert)
        {
            Cars[car.Id] = car;
            return Task.FromResult(car);
        }
    }
}
