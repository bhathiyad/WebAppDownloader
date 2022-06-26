using ApplicationServices.Downloader.Models;
using Microsoft.Extensions.Configuration;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationServices.Downloader
{
    public class DownloadService : IDownloadService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        List<string> visitedUrls = new List<string>();
        HttpResponseMessage httpResponse;
        private readonly string baseUrl;
        private readonly string mainDirectoryPath;

        List<FileModel> fileModels = new List<FileModel>();
        public DownloadService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            baseUrl = _configuration.GetSection("WebAppUrl:BaseUrl").Get<string>();
            mainDirectoryPath = _configuration.GetSection("Directories:MainDirectory").Get<string>();
        }
        public async Task StartDownload()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("File download started...");
            var urls = new List<string>() {""};
            await Process(urls);
            var fileModeltasks = fileModels.Select(fileModel => SaveFile(fileModel));
            await Task.WhenAll(fileModeltasks);
            stopWatch.Stop();
            Console.WriteLine("File download Completed...");
            Console.WriteLine($"{visitedUrls.Count} Urls processed");
            Console.WriteLine(stopWatch.Elapsed);
        }

        private async Task Process(List<string> urls)
        {
            foreach (var url in urls)
            {
                if (!visitedUrls.Contains(url) && !url.Contains("http"))
                {
                    var responseString = await GetWebPageContent(baseUrl, url).ConfigureAwait(false);

                    visitedUrls.Add(url); 
                    Console.WriteLine($"destination count : {visitedUrls.Count}");

                    if (httpResponse.RequestMessage.RequestUri.ToString().Contains("404"))
                    {
                        continue;
                    }

                    fileModels.Add(new FileModel
                    {
                        FileContent = responseString,
                        Directory = mainDirectoryPath,
                        Url = url
                    });

                    //await Task.Run(() => SaveFile(responseString, url, mainDirectory)).ConfigureAwait(false);

                    var newUrls = HelperExtensions.GetNewUrls(responseString);
                    Console.WriteLine($"Found new urls {newUrls.Count} in {url}");

                    if (newUrls.Count > 0)
                    {
                        await Process(newUrls);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private async Task<string> GetWebPageContent(string baseUrl, string urlPath)
        {
            var url = !string.IsNullOrWhiteSpace(urlPath) ? 
                            $"{baseUrl}{urlPath[9..(urlPath.Length - 1)]}" : baseUrl;

            Console.WriteLine($"Processing url : {url}");

            httpResponse = await _httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseString;
            }

            return null;
        }

        private async Task SaveFile(FileModel fileModel)
        {
            var fileName = HelperExtensions.GetFileName(fileModel.Url);

            var folderPath = HelperExtensions.PathCombine(fileModel.Directory, fileName);
            var directoryPath = Path.GetDirectoryName(folderPath);
            folderPath = folderPath.Replace("/", "\\");

            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter outputFile = new StreamWriter(folderPath + ".html"))
                {
                    await outputFile.WriteAsync(fileModel.FileContent).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
