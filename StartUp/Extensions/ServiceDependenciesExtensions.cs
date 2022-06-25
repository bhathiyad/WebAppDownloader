using ApplicationServices.Downloader;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace StartUp.Extensions
{
    public static class ServiceDependenciesExtensions
    {
        public static void AddServiceDependencies(this IServiceCollection services) 
        {
            services.AddHttpClient();
            services.AddSingleton<Downloader>();
            services.AddSingleton<IDownloadService, DownloadService>();
        }
    }
}
