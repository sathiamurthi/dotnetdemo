using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace CqrsMediatrExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //Initialize Logger    
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341")
                .ReadFrom.Configuration(config)

                .CreateLogger();
            try
            {
                Log.Information("Application Starting.##############3");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.######333");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

		public static IHostBuilder CreateHostBuilder(string[] args) =>

			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();

                }).UseSerilog();

	}
}
