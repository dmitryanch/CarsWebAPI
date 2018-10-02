using CarsApp.Model.DTO;
using MongoDB.Bson.Serialization.Attributes;

namespace CarsApp.Model
{
    [BsonDiscriminator("IntegerFieldValue")]
    public sealed class IntegerFieldValue : FieldValue<int>
    {
        public override int[] Values { get; }
        public override FieldType Type => FieldType.Integer;

        public IntegerFieldValue(int fieldId, int[] values) : base(fieldId) =>
            Values = values;
    }
}
