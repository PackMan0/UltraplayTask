using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                   .UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureAppConfiguration((hostingContext, config) =>
                                              {
                                                  var env = hostingContext.HostingEnvironment;
                                                  var businessConfigFilePath = Path.Combine(env.ContentRootPath, "..", "BusinessLayer", "businessConfiguration.json");
                                                  var dataAccessConfigFilePath = Path.Combine(env.ContentRootPath, "..", "DataAccessLayer", "dataAccessConfiguration.json");

                                                  config.AddJsonFile(businessConfigFilePath, optional: true, reloadOnChange: true) // When running using dotnet run
                                                        .AddJsonFile("businessConfiguration.json", optional: true, reloadOnChange: true) // When app is published
                                                        .AddJsonFile(dataAccessConfigFilePath, optional: true, reloadOnChange: true) // When running using dotnet run
                                                        .AddJsonFile("dataAccessConfiguration.json", optional: true, reloadOnChange: true) // When app is published
                                                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                                                  config.AddEnvironmentVariables();
                                              })
                   .UseStartup<Startup>();
        }
    }
}
