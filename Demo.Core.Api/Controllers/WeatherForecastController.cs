using Microsoft.AspNetCore.Mvc;
using Demo.Core.Domain.Models;
using Demo.Core.Api.Extensions;
using Microsoft.AspNetCore.Cors;

namespace Demo.Application.Api.Controllers
{
    [ApiController]
   
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [EnableCors("AllowOrigin")]

        [HttpGet("weatherforecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                DownloadUrl = Helper.GetBaseUrl(Request) + "/download/" + new Random().Next(1, 25).ToString()
            })
            .ToArray();
        }
    }
}
