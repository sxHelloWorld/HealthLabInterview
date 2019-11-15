using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Database.Abstractions;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntervieweeProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherAccess _weatherAccess;
        public WeatherForecastController(IWeatherAccess weatherAccess)
        {
            _weatherAccess = weatherAccess;
        }

        [HttpGet("GetByZipcode/{zipcode}")]
        public async Task<IActionResult> GetByZipcodeAsync([FromRoute] string zipcode)
        {
            WeatherForecast forecast = await _weatherAccess.GetForecastByZipcode(zipcode);
            if (forecast.TemperatureC < 5)
            {
                forecast.Summary = "Bundle up it's really cold out!";
            }
            else if (forecast.TemperatureC < 15)
            {
                forecast.Summary = "Sweater weather!";
            }
            else
            {
                forecast.Summary = "Nice and warm";
            }

            return new JsonResult(forecast);
        }
    }
}
