namespace CarsApp.Model.DTO
{
    public class FieldValueDTO
    {
        public int FieldId { get; set; }
        public string Values { get; set; }
        public FieldType Type { get; set; }

        public FieldValueDTO(int fieldId, string values, FieldType type) =>
            (FieldId, Values, Type) = (fieldId, values, type);
    }
}
