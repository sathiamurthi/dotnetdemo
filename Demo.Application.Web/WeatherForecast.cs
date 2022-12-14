using System;

namespace Demo.Application.Web
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public string DownloadUrl { get; set; }
    }
    public partial class Employee
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public int Id { get; set; }
    }

    // API shakehand object
    public class APIResponse
    {
        public string? Success { get; set; }
        public string? Message { get; set; }
        public dynamic[]? Data { get; set; }
        public string? ErrorCode { get; set; }

    }
}
