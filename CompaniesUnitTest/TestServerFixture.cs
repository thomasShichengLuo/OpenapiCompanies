using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using OpenapiCompanies;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using OpenapiCompanies.Companies;

namespace CompaniesUnitTest
{
    public class TestServerFixture : WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseEnvironment("Testing")
                              .ConfigureAppConfiguration(builder =>
                              {
                                  var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                                  builder.AddJsonFile(configPath);

                              });
                });

            return hostBuilder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
               
            });

            base.ConfigureWebHost(builder);
        }
    }
}