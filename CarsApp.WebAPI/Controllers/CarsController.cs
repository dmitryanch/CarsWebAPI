using CarsApp.Model;
using CarsApp.Model.DTO;
using CarsApp.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarsWebAPI.Controllers
{
    [Route("api/cars")]
    public class CarsController : Controller
    {
        #region Private Fields
        ICarsService _dataService;
        private readonly ILogger _logger;
        private FieldsConfig _fieldConfig;
        #endregion

        public CarsController(ILoggerFactory loggerFactory, ICarsService dbContext,
            IOptions<FieldsConfig> fieldsConfigOptions) =>
            (_logger, _dataService, _fieldConfig) =
            (loggerFactory.CreateLogger<CarsController>(), dbContext, fieldsConfigOptions.Value);

        #region Public API
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug("[Get] was invoked on Cars Controller");
            var cars = await _dataService.Get();
            return Ok(cars?.Select(c => c.ToDTO()));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ObjectId id)
        {
            _logger.LogDebug("[Get] was invoked on Cars Controller");
            var car = await _dataService.Get(id);
            return Ok(car?.ToDTO());
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CarDTO dto)
        {
            _logger.LogDebug("[Post] was invoked on Cars Controller");
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Required fields are missing");
            }
            var car = _fieldConfig.Parse(dto);
            await _dataService.Insert(car);
            return Ok(car?.ToDTO());
        }

        // PUT api/<controller>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(ObjectId id, [FromBody]CarDTO dto)
        {
            _logger.LogDebug("[Put] was invoked on Cars Controller");
            var car = _fieldConfig.Parse(dto, false);
            foreach(var fv in car.FieldValues)
            {
                bool Filter(Car c) => c.Id == car.Id && c.FieldValues.Any(f => f.FieldId == fv.FieldId);
                await _dataService.Update(id, Filter, c => c.FieldValues.ElementAt(-1), fv);
            }
            
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ObjectId id)
        {
            _logger.LogDebug("[Delete] was invoked on Cars Controller");
            await _dataService.Remove(id);
            return Ok();
        }

        // DELETE api/<controller>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]ObjectId[] ids)
        {
            _logger.LogDebug("[Delete] multiple records was invoked on Cars Controller");
            await _dataService.Remove(ids);
            return Ok();
        }
        #endregion
    }
}
