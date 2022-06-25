using Microsoft.Extensions.Configuration;
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

        List<TaskFile> taskFiles = new List<TaskFile>();
        public DownloadService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task StartDownload()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var urls = new List<string>() {""};
            await Process(urls);
            var tasks = taskFiles.Select(f => SaveFile(f));
            await Task.WhenAll(tasks);
            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);
        }

        private async Task Process(List<string> urls)
        {
            var baseUrl = _configuration.GetSection("WebAppUrl:BaseUrl").Get<string>();
            var mainDirectory = _configuration.GetSection("Directories:MainDirectory").Get<string>();

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

                    taskFiles.Add(new TaskFile
                    {
                        Response = responseString,
                        Directory = mainDirectory,
                        Url = url
                    });

                    //await Task.Run(() => SaveFile(responseString, url, mainDirectory)).ConfigureAwait(false);

                    var newUrls = GetNewUrls(responseString);
                    Console.WriteLine(newUrls.Count);

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

            httpResponse = await _httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            if (httpResponse.IsSuccessStatusCode)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                return responseString;
            }

            return null;
        }

        private async Task SaveFile(string responseString, string url, string mainDirectory) 
        {
            var fileName = !string.IsNullOrWhiteSpace(url) ? url[9..(url.Length - 1)] : "main";

            var folderPath = PathCombine(mainDirectory, fileName);
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
                    await outputFile.WriteAsync(responseString).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SaveFile(TaskFile taskFile)
        {
            var fileName = !string.IsNullOrWhiteSpace(taskFile.Url) ? taskFile.Url[9..(taskFile.Url.Length - 1)] : "main";

            var folderPath = PathCombine(taskFile.Directory, fileName);
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
                    await outputFile.WriteAsync(taskFile.Response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> GetNewUrls(string responseString) 
        {
            var pattern = "<a href=\".*?\"";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var newUrls = regex.Matches(responseString).Select(a => a.Value).ToList();

            return newUrls;
        }

        private string PathCombine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }

        public class TaskFile 
        {
            public string Response { get; set; }
            public string Url { get; set; }
            public string Directory { get; set; }
        }
    }

}
