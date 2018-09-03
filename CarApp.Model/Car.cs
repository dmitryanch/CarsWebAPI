using CarApp.Model.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CarApp.Model
{
    public class Car : IEntity<int>
    {
        [BsonId]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
