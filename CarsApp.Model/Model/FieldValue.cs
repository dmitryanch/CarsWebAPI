using CarsApp.Model.DTO;
using CarsApp.Model.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace CarsApp.Model
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(IntegerFieldValue), typeof(StringFieldValue))]
    public abstract class FieldValue : IFieldValue
    {
        public int FieldId { get; }
        public abstract FieldType Type { get; }
        public abstract string StringValues { get; }

        protected FieldValue(int fieldId) =>
           FieldId = fieldId;
    }

    public abstract class FieldValue<T> : FieldValue, IFieldValue<T>
    {
        public abstract T[] Values { get; }
        public override string StringValues => Values == null ? null : string.Join("\\,", Values);

        protected FieldValue(int fieldId) : base(fieldId) {}

    }
}
