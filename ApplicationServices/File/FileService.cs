using ApplicationServices.Downloader.Models;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices.File
{
    public class FileService : IFileService
    {
        public async Task SaveFileToDisk(FileModel fileModel)
        {
            var fileName = HelperExtensions.GetFileName(fileModel.Url);

            var folderPath = HelperExtensions.PathCombine(fileModel.Directory, fileName);
            var directoryPath = Path.GetDirectoryName(folderPath);
            folderPath = folderPath.Replace("/", "\\");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (StreamWriter outputFile = new StreamWriter(folderPath + ".html"))
            {
                await outputFile.WriteAsync(fileModel.FileContent).ConfigureAwait(false);
            }

        }
    }
}
