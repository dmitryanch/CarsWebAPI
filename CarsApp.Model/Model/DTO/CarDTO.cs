using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CarsApp.Model.DTO
{
    public class CarDTO
    {
        public ObjectId Id { get; set; }
        [Required]
        public FieldValueDTO[] FieldValues { get; set; }

        public CarDTO() { }

        public CarDTO(ObjectId id, FieldValueDTO[] fieldValueDtos) =>
            (Id, FieldValues) = (id, fieldValueDtos);
    }
}
