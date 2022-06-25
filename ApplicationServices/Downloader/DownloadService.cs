using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Downloader
{
    public class DownloadService : IDownloadService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public DownloadService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task StartDownload()
        {
            var baseUrl = _configuration.GetSection("WebAppUrl:BaseUrl").Get<string>();
            var httpResponse = await _httpClientFactory.CreateClient().GetAsync(baseUrl);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync();
            }
        }
    }
}
