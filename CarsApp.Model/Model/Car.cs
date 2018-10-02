using CarsApp.Model.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace CarsApp.Model
{
    public class Car : IEntity<ObjectId>
    {
        [BsonId]
        public ObjectId Id { get; }
        public IEnumerable<IFieldValue> FieldValues { get; set; }
        [BsonIgnore]
        private IReadOnlyDictionary<int, IFieldValue> _feldValueTable;
        [BsonIgnore]
        public IReadOnlyDictionary<int, IFieldValue> FieldValueTable =>
            _feldValueTable ?? (_feldValueTable = FieldValues.ToDictionary(fv => fv.FieldId));

        public Car()
        {
            Id = ObjectId.GenerateNewId();
        }

        public Car(ObjectId id, IEnumerable<IFieldValue> fvs) =>
            (Id, FieldValues) = (id == default(ObjectId) ? ObjectId.GenerateNewId() : id, fvs);
    }
}
