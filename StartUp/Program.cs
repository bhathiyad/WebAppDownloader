using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StartUp.Extensions;
using System;
using System.Threading.Tasks;

namespace StartUp
{
    class Program
    {
        static IConfiguration configuration;
        static async Task Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

            Console.WriteLine("Hello World!");

            var services = new ServiceCollection();
            services.AddServiceDependencies(configuration);
            services.AddHttpClients(configuration);

            var downloader = services.BuildServiceProvider().GetService<Downloader>();
            await downloader.Download();
        }
    }
}
