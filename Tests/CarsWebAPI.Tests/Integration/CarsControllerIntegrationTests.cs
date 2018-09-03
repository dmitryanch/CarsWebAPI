using CarApp.Model;
using CarApp.Model.Interfaces;
using CarsWebAPI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mongo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarsWebAPI.Tests.Integration
{
    [TestClass]
    public class CarsControllerIntegrationTests
    {
        private IServiceProvider ServiceProvider { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            ServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ICarsService, MongoCarsDataService>(s =>
                    new MongoCarsDataService(config["ConnectionStrings:Mongo"], config["MongoSettings:testDb"]))
                .BuildServiceProvider();
        }

        [TestCleanup]
        public void CleanDb()
        {
            var ds = ServiceProvider.GetService<ICarsService>();
            ds.Remove(new int[] { 1, 2, 3, 4, 5 });
        }

        [TestMethod]
        public async Task Post_CreatedCarIsNotNull()
        {
            var newCar = new Car
            {
                Id = 1,
                Name = "TestName"
            };
            var controller = new CarsController(ServiceProvider.GetService<ILoggerFactory>(),
                ServiceProvider.GetService<ICarsService>());
            var createdCar = await controller.Post(newCar);
            Assert.IsNotNull(createdCar);
        }

        [TestMethod]
        public async Task Put_UpdateCar()
        {
            var newCar = new Car
            {
                Id = 3,
                Name = "TestName"
            };
            var controller = new CarsController(ServiceProvider.GetService<ILoggerFactory>(),
                ServiceProvider.GetService<ICarsService>());
            var createdCar = await controller.Post(newCar);
            Assert.IsNotNull(createdCar);
            Assert.IsNull(createdCar.Description);
            Assert.AreEqual(createdCar.Name, "TestName");
            await controller.Put(3, new Car
            {
                Name = "ModifiedName",
                Description = "New Description"
            });
            var car = await controller.Get(3);
            Assert.AreEqual(car.Name, "ModifiedName");
            Assert.AreEqual(car.Description, "New Description");
        }

        [TestMethod]
        public async Task Delete_DeleteCar()
        {
            var newCar = new Car
            {
                Id = 4,
                Name = "TestName3"
            };
            var controller = new CarsController(ServiceProvider.GetService<ILoggerFactory>(),
                ServiceProvider.GetService<ICarsService>());
            var allCars = await controller.Get();
            var initialCarsCount = allCars.Count();
            var createdCar = await controller.Post(newCar);
            Assert.IsNotNull(createdCar);
            Assert.AreEqual(initialCarsCount, (await controller.Get()).Count() - 1);
            await controller.Delete(4);
            var car = await controller.Get(4);
            Assert.IsNull(car);
            Assert.AreEqual(initialCarsCount, (await controller.Get()).Count());
        }

        [TestMethod]
        public async Task Delete_DeleteCars()
        {
            var cars = new[]
            {
                new Car
                {
                    Id = 5,
                    Name = "TestName"
                },
                new Car
                {
                    Id = 6,
                    Name = "TestName2",
                    Description = "Some Text"
                },
                new Car
                {
                    Id = 7,
                    Name = "Test 3"
                }
            };

            var controller = new CarsController(ServiceProvider.GetService<ILoggerFactory>(),
                ServiceProvider.GetService<ICarsService>());
            var allCars = await controller.Get();
            var initialCarsCount = allCars.Count();

            await Task.WhenAll(cars.Select(c => controller.Post(c)));

            Assert.AreEqual(initialCarsCount, (await controller.Get()).Count() - 3);
            await controller.Delete(new[] { 5, 6, 7});
            Assert.IsNull(await controller.Get(5));
            Assert.IsNull(await controller.Get(6));
            Assert.IsNull(await controller.Get(7));
            Assert.AreEqual(initialCarsCount, (await controller.Get()).Count());
        }
    }
}
