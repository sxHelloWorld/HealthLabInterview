using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class WeatherReportViewModel
    {
        public string Zipcode { get; set; }
        public string TemperatureC { get; set; }

        public string TemperatureF { get; set; }

        public string Description { get; set; }

        public string TemperatureClass { get; set; }
    }
}
