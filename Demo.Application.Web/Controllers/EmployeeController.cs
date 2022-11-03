using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.Application.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {


        private readonly ILogger<EmployeeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string apiUrl;

        public EmployeeController(ILogger<EmployeeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            apiUrl = _configuration.GetValue<string>("API:local");
        }

        [HttpGet]
        public async Task<APIResponse> Get([FromQuery] int startPage)
        {
            HttpClient client = new HttpClient();
            dynamic model = await client.GetFromJsonAsync<APIResponse>(apiUrl + "employees" + "?" + "startPage=" + startPage.ToString());
            //IEnumerable<WeatherForecast> forecasts =  client.GetAsync("http://localhost:5263/WeatherForecast/weatherforecast") as IEnumerable<WeatherForecast>;
            return model;
        }


        [HttpGet]
        [Route("detail/{id}")]
        public async Task<Employee> Details([FromRoute] string id)
        {
            HttpClient client = new HttpClient();
            var model = await client.GetFromJsonAsync<APIResponse>(apiUrl + "employees/" + id);
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Employee>(model.Data[0].ToString());
            return json;
        }


        [HttpPost]
        [Route("create")]
        public async Task<int> Create([FromBody] Employee employee)
        {
            HttpClient client = new HttpClient();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(employee);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var model = await client.PostAsync(apiUrl + "employees/Create", data);
            //IEnumerable<WeatherForecast> forecasts =  client.GetAsync("http://localhost:5263/WeatherForecast/weatherforecast") as IEnumerable<WeatherForecast>;
          
            return 0;
        }

        [HttpPut]
        [Route("update")]
        public async Task<int> Update([FromBody] Employee employee)
        {
            HttpClient client = new HttpClient();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(employee);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            var model = await client.PutAsync(apiUrl + "employees/Update", data);
            //IEnumerable<WeatherForecast> forecasts =  client.GetAsync("http://localhost:5263/WeatherForecast/weatherforecast") as IEnumerable<WeatherForecast>;

            return (int) model.StatusCode;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<int> Delete([FromRoute] string id)
        {
            HttpClient client = new HttpClient();
            var model = await client.DeleteAsync(apiUrl + "employees/delete/" + id);
            //IEnumerable<WeatherForecast> forecasts =  client.GetAsync("http://localhost:5263/WeatherForecast/weatherforecast") as IEnumerable<WeatherForecast>;

            return 0;
        }

        [HttpGet]
        [Route("cities")]
        public async Task<APIResponse> Cities()
        {
            HttpClient client = new HttpClient();
            var model = await client.GetFromJsonAsync<APIResponse>(apiUrl + "cities");
            //IEnumerable<WeatherForecast> forecasts =  client.GetAsync("http://localhost:5263/WeatherForecast/weatherforecast") as IEnumerable<WeatherForecast>;

            return model;
        }

    }
}
