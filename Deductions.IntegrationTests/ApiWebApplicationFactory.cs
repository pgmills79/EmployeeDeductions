using System.IO;
using Deductions.API;
using Deductions.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deductions.IntegrationTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
            // will be called after the `ConfigureServices` from the Startup
            builder.ConfigureTestServices(services =>
            {
                //services.AddTransient(_ => new SqlConnection(Configuration.GetConnectionString("LabOps_Local")));

                //only use this service in integration tests at the moment.  Everything else should be done through API calls
                services.AddTransient<IEmployeeRepository, EmployeeRepository>();
                services.AddTransient<ISpouseRepository, SpouseRepository>();
            });
        }
    }
}