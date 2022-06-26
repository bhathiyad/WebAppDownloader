using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Helpers
{
    public static class HelperExtensions
    {
        public static List<string> GetNewUrls(string httpResponseString)
        {
            var pattern = "<a href=\".*?\"";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var newUrls = regex.Matches(httpResponseString).Select(a => a.Value).ToList();

            return newUrls;
        }

        public static string GetFileName(string url, string defaultName = "main")
        {
            var fileName = !(string.IsNullOrWhiteSpace(url) || url == "<a href=\"/\"") ? url[9..(url.Length - 1)] : defaultName;

            return fileName;
        }

        public static string PathCombine(string path1, string path2)
        {
            if (Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1, path2);
        }
    }
}
