using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Downloader
{
    public class DownloadService : IDownloadService
    {
        public async Task StartDownload()
        {
            Console.WriteLine("Download Started");
        }
    }
}
