using System.IO;
using Deductions.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Deductions.IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private IConfiguration Configuration { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build();

                config.AddConfiguration(Configuration);
               
            });

        }

    }
}