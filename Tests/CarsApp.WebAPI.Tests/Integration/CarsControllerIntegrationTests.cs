using CarsApp.Model;
using CarsApp.Model.Interfaces;
using CarsApp.MongoORM;
using CarsApp.WebAPI.Tests.UnitTests;
using CarsWebAPI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace CarsWebAPI.Tests.Integration
{
    [TestClass]
    public class CarsControllerIntegrationTests : CarsControllerTestsBase
    {
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
                .Configure<FieldsConfig>(fc =>
                {
                    fc.Fields = new[]
                    {
                        new Field
                        {
                            Id = 1,
                            Name = "Name",
                            Options = new []{ FieldOption.Required }
                        },
                        new Field {
                            Id = 2,
                            Name = "Description",
                            Options = new FieldOption[0]
                        }
                    };
                })
                .AddSingleton<CarsController>()
                .BuildServiceProvider();
        }

        [TestCleanup]
        public async Task CleanDb()
        {
            var ds = ServiceProvider.GetService<ICarsService>();
            var cars = await ds.Get();
            await ds.Remove(cars.Select(c => c.Id).ToArray());
        }
    }
}
