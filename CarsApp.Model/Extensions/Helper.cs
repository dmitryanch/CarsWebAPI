using CarsApp.Model.DTO;
using CarsApp.Model.Interfaces;
using System;
using System.Linq;

namespace CarsApp.Model
{
    public static class Extensions
    {
        public static Car Parse(this FieldsConfig config, CarDTO dto, bool strict = true)
        {
            var requiredFields = config.Fields.Where(f => f.Options?.Contains(FieldOption.Required) ?? false)
                .Select(f => f.Id).ToArray();
            var fieldValues = dto.FieldValues.Select(fv => fv.Parse(config.Fields.First(f => f.Id == fv.FieldId).Options)).ToArray();
            if (strict && requiredFields.Any(rf => !fieldValues.Select(fv => fv.FieldId).Contains(rf)))
            {
                throw new InvalidOperationException("Missing Required Fields");
            }
            return new Car(dto.Id, fieldValues);
        }

        public static IFieldValue Parse(this FieldValueDTO dto, FieldOption[] options)
        {
            if (dto == null) return null;
            var values = options.Contains(FieldOption.MultiSelect)
                ? dto.Values.Split(new[] { "\\," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray()
                : new[] { dto.Values };
            if (options.Contains(FieldOption.Required) && !values.Any())
            {
                throw new InvalidOperationException("Missing Required Fields");
            }
            switch (dto.Type)
            {
                case FieldType.Integer:
                    return new IntegerFieldValue(dto.FieldId, values.Select(s => int.Parse(s)).ToArray());
                case FieldType.String:
                    return new StringFieldValue(dto.FieldId, values.ToArray());
                default:
                    return null;
            }
        }

        public static CarDTO ToDTO(this Car car)
        {
            return new CarDTO(car.Id, car.FieldValues.Select(ToDTO).ToArray());
        }

        public static FieldValueDTO ToDTO(this IFieldValue fieldValue)
        {
            return new FieldValueDTO(fieldValue.FieldId, fieldValue.StringValues, fieldValue.Type);
        }
    }
}
