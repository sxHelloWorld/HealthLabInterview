using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database.Abstractions
{
    public interface IWeatherAccess
    {
        Task<WeatherForecast> GetForecastByZipcode(string zipcode);
    }
}
