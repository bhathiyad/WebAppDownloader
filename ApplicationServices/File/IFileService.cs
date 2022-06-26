using ApplicationServices.Downloader.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.File
{
    public interface IFileService
    {
        Task SaveFileToDisk(FileModel fileModel);
    }
}
