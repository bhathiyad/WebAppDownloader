using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.Downloader.Models
{
    public class FileModel
    {
        public string FileContent { get; set; }
        public string Directory { get; set; }
        public string Url { get; set; }
    }
}
