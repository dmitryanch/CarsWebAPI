using CarApp.Model;
using CarApp.Model.Extensions;
using CarApp.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsWebAPI.Controllers
{
    [Route("api/cars")]
    public class CarsController : Controller
    {
        #region Private Fields
        ICarsService _dataService;
        private readonly ILogger _logger;
        #endregion

        public CarsController(ILoggerFactory loggerFactory, ICarsService dbContext)
        {
            _logger = loggerFactory.CreateLogger<CarsController>();
            _dataService = dbContext;
        }

        #region Public API
        // GET: api/<controller>
        [HttpGet]
        public async Task<IEnumerable<Car>> Get()
        {
            _logger.LogDebug("[Get] was invoked on Cars Controller");
            return await _dataService.Get();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<Car> Get(int id)
        {
            _logger.LogDebug("[Get] was invoked on Cars Controller");
            return await _dataService.Get(id);
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<Car> Post([FromBody]Car car)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Required fields are missing");
            }
            _logger.LogDebug("[Post] was invoked on Cars Controller");
            return await _dataService.Save(car, true);
        }

        // PUT api/<controller>
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Car car)
        {
            _logger.LogDebug("[Put] was invoked on Cars Controller");
            var existingCar = await _dataService.Get(id);
            existingCar.MergeWith(car);
            await _dataService.Save(existingCar, false);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            _logger.LogDebug("[Delete] was invoked on Cars Controller");
            await _dataService.Remove(id);
        }

        // DELETE api/<controller>
        [HttpDelete]
        public async Task Delete([FromBody]int[] ids)
        {
            _logger.LogDebug("[Delete] multiple records was invoked on Cars Controller");
            await _dataService.Remove(ids);
        }
        #endregion
    }
}
