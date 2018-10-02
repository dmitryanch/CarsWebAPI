using MongoDB.Bson;

namespace CarsApp.Model.Interfaces
{
    public interface ICarsService : IDataService<Car, ObjectId>
    {
    }
}
