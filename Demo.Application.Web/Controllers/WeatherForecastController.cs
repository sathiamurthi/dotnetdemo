using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace Demo.Application.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string apiUrl;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            apiUrl = _configuration.GetValue<string>("API:local");
        } 

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            HttpClient client = new HttpClient();
            var  model =   await client.GetFromJsonAsync<IEnumerable<WeatherForecast>>(apiUrl + "weatherforecast");
            //IEnumerable<WeatherForecast> forecasts =  client.GetAsync("http://localhost:5263/WeatherForecast/weatherforecast") as IEnumerable<WeatherForecast>;
            return model;
        }
    }
}
