using System;
using System.Collections.Generic;
using CkoShoppingList.Service.Exceptions;
using CkoShoppingList.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CkoShoppingList.Service.Controllers.api
{
    [Route("api/drinks")]
    public class DrinksController : Controller
    {
        private readonly IStorageService _storageService;
        private readonly ILogger _logger;

        public DrinksController(IStorageService storageService, ILoggerFactory loggerFactory)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _logger = loggerFactory?.CreateLogger(nameof(DrinksController)) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var drinks = _storageService.GetDrinks();

                return Ok(drinks);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Invalid name.");
                }

                var drink = _storageService.GetDrink(name);

                return Ok(drink);
            }
            catch (ItemNotFoundException e)
            {
                _logger.LogError(0, e, e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] KeyValuePair<string, int> drink)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(drink.Key))
                {
                    return BadRequest("Invalid name.");
                }

                if (drink.Value <= 0)
                {
                    return BadRequest("Invalid value.");
                }

                var addedDrink = _storageService.AddDrink(drink);

                return Created("/", addedDrink);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] KeyValuePair<string, int> drink)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Invalid name.");
                }

                if (drink.Value <= 0)
                {
                    return BadRequest("Invalid value.");
                }

                var updatedDrink = _storageService.UpdateDrink(name, drink);

                return Ok(updatedDrink);
            }
            catch (ItemNotFoundException e)
            {
                _logger.LogError(0, e, e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Invalid name.");
                }

                _storageService.DeleteDrink(name);

                return NoContent();
            }
            catch (ItemNotFoundException e)
            {
                _logger.LogError(0, e, e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
