using ApplicationServices.Downloader;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StartUp
{
    public class Downloader
    {
        private readonly IDownloadService _downloadService;

        public Downloader(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }
        public async Task Download() 
        {
            await _downloadService.StartDownload();
        }
    }
}
