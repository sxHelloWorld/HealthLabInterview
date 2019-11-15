using API.Database.Abstractions;
using API.Configuration;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Database
{
    // Access classes are used to pull in data from the database and create any models you may need for the logic done within the managers.
    // In this example there is no complicated relationships to create so this is a fairly simple retrieval from the database using Entity Framework.
    public class WeatherAccess : IWeatherAccess
    {
        public async Task<WeatherForecast> GetForecastByZipcode(string zipcode)
        {
            using(var db = new DatabaseConfiguration())
            {
                return await db.Forecasts.SingleAsync(f => f.Zipcode == zipcode);
            }
        }
    }
}
