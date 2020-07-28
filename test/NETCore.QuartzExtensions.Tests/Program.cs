using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace NETCore.QuartzExtensions.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddQuartz<TestJob>(options=>
                    {
                        options.ThreadCount = 10;

                    }).AddScoped<TestJob2>();
                    services.AddHostedService<HostService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddConsole();
                });

            await hostBuilder.Build().RunAsync();
        }
    }
}
