using ApplicationServices.Downloader.Models;
using ApplicationServices.File;
using ApplicationServices.RequestManager;
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
        private readonly IRequestManagerService _requestManagerService;
        private readonly IFileService _fileService;
        private readonly List<string> visitedUrls = new List<string>();
        private readonly string baseUrl;
        private readonly string mainDirectoryPath;
        private readonly List<FileModel> fileModels = new List<FileModel>();

        public DownloadService(IConfiguration configuration
            , IRequestManagerService requestManagerService
            , IFileService fileService)
        {
            _configuration = configuration;
            _requestManagerService = requestManagerService;
            _fileService = fileService;
            baseUrl = _configuration.GetSection("WebAppUrl:BaseUrl").Get<string>();
            mainDirectoryPath = _configuration.GetSection("Directories:MainDirectory").Get<string>();
        }
        public async Task StartDownload()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("File download started...");

            var urls = new List<string>() { "" };
            await Process(urls);
            var fileModeltasks = fileModels.Select(fileModel => _fileService.SaveFileToDisk(fileModel));
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
                    var response = await _requestManagerService.GetWebPageContent(baseUrl, url);

                    visitedUrls.Add(url);

                    if (response.httpResponseMessage.RequestMessage.RequestUri.ToString().Contains("404"))
                    {
                        continue;
                    }

                    fileModels.Add(new FileModel
                    {
                        FileContent = response.httpResponseString,
                        Directory = mainDirectoryPath,
                        Url = url
                    });

                    //await Task.Run(() => SaveFile(responseString, url, mainDirectory)).ConfigureAwait(false);

                    var newUrls = HelperExtensions.GetNewUrls(response.httpResponseString);
                    Console.WriteLine($"Found new urls {newUrls.Count}");

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

    }

}
