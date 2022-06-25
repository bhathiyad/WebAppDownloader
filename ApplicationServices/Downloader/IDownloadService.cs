using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.Downloader
{
    public interface IDownloadService
    {
        Task StartDownload();
    }
}
