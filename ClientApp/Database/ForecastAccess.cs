using ClientApp.Database.Abstractions;
using ClientApp.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.Database
{
    public class ForecastAccess : IForecastAccess
    {
        private readonly IHttpClientFactory _clientFactory;

        public ForecastAccess(IHttpClientFactory factory)
        {
            _clientFactory = factory;
        }

        public async Task<WeatherForecast> GetForecastForZipcode(string zipcode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:5001/WeatherForecast/GetByZipcode/{zipcode}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            WeatherForecast toRet;

            if(response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                toRet = await JsonSerializer.DeserializeAsync<WeatherForecast>(stream);
            } else
            {
                throw new Exception("Forecast request failed");
            }

            return toRet;
        }
    }
}
