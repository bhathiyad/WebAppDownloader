using Microsoft.Extensions.DependencyInjection;
using StartUp.Extensions;
using System;
using System.Threading.Tasks;

namespace StartUp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var services = new ServiceCollection();
            services.AddServiceDependencies();

            var downloader = services.BuildServiceProvider().GetService<Downloader>();
            await downloader.Download();
        }
    }
}
