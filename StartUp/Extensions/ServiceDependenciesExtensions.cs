using ApplicationServices.Downloader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace StartUp.Extensions
{
    public static class ServiceDependenciesExtensions
    {
        public static void AddServiceDependencies(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton(configuration);
            services.AddHttpClient();
            services.AddSingleton<Downloader>();
            services.AddSingleton<IDownloadService, DownloadService>();
        }
    }
}
