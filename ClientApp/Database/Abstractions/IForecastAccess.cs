using ClientApp.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Database.Abstractions
{
    public interface IForecastAccess
    {
        Task<WeatherForecast> GetForecastForZipcode(string zipcode);
    }
}
