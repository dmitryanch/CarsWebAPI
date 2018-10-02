using CarsApp.Model;
using CarsApp.Model.Interfaces;
using CarsApp.WebAPI.Tests.UnitTests;
using CarsWebAPI.Controllers;
using CarsWebAPI.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarsWebAPI.Tests
{
    [TestClass]
    public class CarsControllerTests : CarsControllerTestsBase
    {
        [TestInitialize]
        public void Setup()
        {
            ServiceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ICarsService, InMemoryCarsService>()
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
                        new Field
                        {
                            Id = 2,
                            Name = "Description",
                            Options = new FieldOption[0]
                        }
                    };
                })
                .AddSingleton<CarsController>()
                .BuildServiceProvider();
        }
    }
}
