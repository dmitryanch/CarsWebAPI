using CarsApp.Model.DTO;
using CarsWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.WebAPI.Tests.UnitTests
{
    public abstract class CarsControllerTestsBase
    {
        protected IServiceProvider ServiceProvider { get; set; }

        [TestMethod]
        public async Task Post_CreatedCarIsNotNull()
        {
            var newCar = new CarDTO
            {
                FieldValues = new[]
                {
                    new FieldValueDTO(1, "Test Name", FieldType.String),
                    new FieldValueDTO(2, "Test Description", FieldType.String)
                }
            };
            var controller = ServiceProvider.GetService<CarsController>();
            var createdCar = (CarDTO)(await controller.Post(newCar) as OkObjectResult).Value;
            Assert.IsNotNull(createdCar);
        }

        [TestMethod]
        public async Task Put_CreateCarWithoutDescription_ThenUpdateCar()
        {
            const int nameFieldId = 1;
            const int descriptionFieldId = 2;
            var newCar = new CarDTO
            {
                FieldValues = new[] { new FieldValueDTO(1, "Test Name", FieldType.String), }
            };
            var controller = ServiceProvider.GetService<CarsController>();
            var createdCar = (CarDTO)(await controller.Post(newCar) as OkObjectResult).Value;

            Assert.IsNotNull(createdCar);
            // Check that there is no description field
            Assert.ThrowsException<InvalidOperationException>(() => createdCar.FieldValues.First(f => f.FieldId == descriptionFieldId));
            // Check that single name field value is ...
            var nameField = createdCar.FieldValues.First(f => f.FieldId == nameFieldId);
            Assert.AreEqual(nameField.Values, "Test Name");

            var putResponse = await controller.Put(createdCar.Id, new CarDTO
            {
                Id = createdCar.Id,
                FieldValues = new[]
                {
                    new FieldValueDTO(1, "Modified Test Name", FieldType.String),
                    new FieldValueDTO(2, "Test Description", FieldType.String),
                }
            }) as OkResult;
            Assert.IsNotNull(putResponse);

            var car = (CarDTO)(await controller.Get(createdCar.Id) as OkObjectResult).Value;
            // Check that Name field value was modified
            var modifiedNameField = car.FieldValues.First(f => f.FieldId == nameFieldId);
            Assert.AreEqual(modifiedNameField.Values, "Modified Test Name");
            // Check that description field value was created
            var descriptionField = car.FieldValues.First(f => f.FieldId == descriptionFieldId);
            Assert.AreEqual(descriptionField.Values, "Test Description");
        }

        [TestMethod]
        public async Task Put_CreateCarWithoutDescription_ThenParticularUpdateCarDescription()
        {
            const int nameFieldId = 1;
            const int descriptionFieldId = 2;
            var newCar = new CarDTO
            {
                FieldValues = new[] { new FieldValueDTO(1, "Test Name", FieldType.String), }
            };
            var controller = ServiceProvider.GetService<CarsController>();
            var createdCar = (CarDTO)(await controller.Post(newCar) as OkObjectResult).Value;

            Assert.IsNotNull(createdCar);
            // Check that single name field value is ...
            var nameField = createdCar.FieldValues.First(f => f.FieldId == nameFieldId);
            Assert.AreEqual(nameField.Values, "Test Name");

            // Then Update Car Description to NotNull FieldValue
            var putResponse = await controller.Put(createdCar.Id, new CarDTO
            {
                Id = createdCar.Id,
                FieldValues = new[] { new FieldValueDTO(2, "Test Description", FieldType.String), }
            }) as OkResult;
            Assert.IsNotNull(putResponse);

            // Check that description field value was created
            var descriptionField = await GetCarFieldValue(controller, createdCar.Id, descriptionFieldId);
            Assert.AreEqual(descriptionField.Values, "Test Description");

            // Then Update Car Description to NotNull FieldValue
            var nextPutResponse = await controller.Put(createdCar.Id, new CarDTO
            {
                Id = createdCar.Id,
                FieldValues = new[] { new FieldValueDTO(2, null, FieldType.String), }
            }) as OkResult;
            Assert.IsNotNull(nextPutResponse);

            // Check that description field value was created
            var updatedDescriptionField = await GetCarFieldValue(controller, createdCar.Id, descriptionFieldId);
            Assert.AreEqual(updatedDescriptionField.Values, null);
        }

        [TestMethod]
        public async Task Delete_DeleteCar()
        {
            var newCar = new CarDTO
            {
                FieldValues = new[]
                {
                    new FieldValueDTO(1, "Test Name", FieldType.String),
                }
            };
            var controller = ServiceProvider.GetService<CarsController>();
            var allCars = (IEnumerable<CarDTO>)(await controller.Get() as OkObjectResult).Value;
            var initialCarsCount = allCars.Count();
            var createdCar = (CarDTO)(await controller.Post(newCar) as OkObjectResult).Value;

            Assert.IsNotNull(createdCar);
            // Check that car count was increased by one after creating a new one
            var carsCountAfter = ((IEnumerable<CarDTO>)(await controller.Get() as OkObjectResult).Value).Count();
            Assert.AreEqual(initialCarsCount, carsCountAfter - 1);

            // Then delete created above car
            var deleteResponse = await controller.Delete(createdCar.Id) as OkResult;
            Assert.IsNotNull(deleteResponse);

            // Check that car was deleted
            var car = (CarDTO)(await controller.Get(createdCar.Id) as OkObjectResult).Value;
            Assert.IsNull(car);

            // Check cars count after deleting
            var carsAfterDeleting = ((IEnumerable<CarDTO>)(await controller.Get() as OkObjectResult).Value).Count();
            Assert.AreEqual(initialCarsCount, carsAfterDeleting);
        }

        [TestMethod]
        public async Task Delete_DeleteCars()
        {
            var cars = new[]
            {
                new CarDTO{FieldValues = new[]{new FieldValueDTO(1, "Test Name1", FieldType.String),}},
                new CarDTO{FieldValues = new[]{new FieldValueDTO(1, "Test Name2", FieldType.String),}},
                new CarDTO{FieldValues = new[]{new FieldValueDTO(1, "Test Name3", FieldType.String),}},
            };

            var controller = ServiceProvider.GetService<CarsController>();
            var initialCarsCount = ((IEnumerable<CarDTO>)(await controller.Get() as OkObjectResult).Value).Count();

            var createdCars = (await Task.WhenAll(cars.Select(c => controller.Post(c))))
                .Cast<OkObjectResult>().Select(c => (CarDTO)c.Value).ToArray();

            var carsCountAfterAdding = ((IEnumerable<CarDTO>)(await controller.Get() as OkObjectResult).Value).Count();

            // Check that cars count was increased
            Assert.AreEqual(initialCarsCount, carsCountAfterAdding - 3);

            // Then delete all added cars
            await controller.Delete(createdCars.Select(c => c.Id).ToArray());

            // Check that above added cars are absent
            Assert.IsNull((CarDTO)(await controller.Get(createdCars[0].Id) as OkObjectResult).Value);
            Assert.IsNull((CarDTO)(await controller.Get(createdCars[1].Id) as OkObjectResult).Value);
            Assert.IsNull((CarDTO)(await controller.Get(createdCars[2].Id) as OkObjectResult).Value);

            // Chech that cars count after deleting equals to initial cars count
            var carsCountAfterDeleting = ((IEnumerable<CarDTO>)(await controller.Get() as OkObjectResult).Value).Count();
            Assert.AreEqual(initialCarsCount, carsCountAfterDeleting);
        }

        #region Private Methods
        private async Task<FieldValueDTO> GetCarFieldValue(CarsController controller, ObjectId id, int fieldId)
        {
            var car = (CarDTO)(await controller.Get(id) as OkObjectResult).Value;
            var fieldValue = car.FieldValues.First(f => f.FieldId == fieldId);
            return fieldValue;
        }
        #endregion
    }
}
