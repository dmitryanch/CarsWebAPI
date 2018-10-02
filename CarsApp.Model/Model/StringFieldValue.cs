using CarsApp.Model.DTO;
using MongoDB.Bson.Serialization.Attributes;

namespace CarsApp.Model
{
    [BsonDiscriminator("StringFieldValue")]
    public sealed class StringFieldValue : FieldValue<string>
    {
        public override string[] Values { get; }
        public override FieldType Type => FieldType.String;

        public StringFieldValue(int fieldId, string[] values) : base(fieldId) =>
            Values = values;
    }
}
