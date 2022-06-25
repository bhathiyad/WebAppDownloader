using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace StartUp.Extensions
{
    public static class HttpClientExtensions
    {
        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddHttpClient();
        }
    }
}
