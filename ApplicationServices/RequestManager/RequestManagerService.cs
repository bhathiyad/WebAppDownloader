using ApplicationServices.RequestManager.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.RequestManager
{
    public class RequestManagerService : IRequestManagerService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RequestManagerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseModel> GetWebPageContent(string baseUrl, string urlPath)
        {
            var url = !string.IsNullOrWhiteSpace(urlPath) ?
                $"{baseUrl}{urlPath[9..(urlPath.Length - 1)]}" : baseUrl;

            Console.WriteLine($"Processing url : {url}");

            var httpResponse = await _httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var httpResponseModel = new ResponseModel
                {
                    httpResponseMessage = httpResponse,
                    httpResponseString = responseString
                };

                return httpResponseModel;
            }

            return null;
        }

        public ResponseModel GetWebPageContentNotAsync(string baseUrl, string urlPath)
        {

            var url = !string.IsNullOrWhiteSpace(urlPath) ?
                $"{baseUrl}{urlPath[9..(urlPath.Length - 1)]}" : baseUrl;

            Console.WriteLine($"Processing url : {url}");

            var httpResponse = _httpClientFactory.CreateClient().GetAsync(url).Result;

            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = httpResponse.Content.ReadAsStringAsync().Result;
                var httpResponseModel = new ResponseModel
                {
                    httpResponseMessage = httpResponse,
                    httpResponseString = responseString
                };

                return httpResponseModel;
            }

            return null;
        }
    }
}
