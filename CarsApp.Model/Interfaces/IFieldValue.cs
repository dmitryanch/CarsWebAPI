using CarsApp.Model.DTO;

namespace CarsApp.Model.Interfaces
{
    public interface IFieldValue
    {
        int FieldId { get; }
        string StringValues { get; }
        FieldType Type { get; }
    }

    public interface IFieldValue<T> : IFieldValue
    {
        T[] Values { get; }
    }
}
