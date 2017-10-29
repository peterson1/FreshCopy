using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public static class FilePathExtensions
    {
        public static string SHA1ForFile(this string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var algo = new HashLib.Crypto.SHA1();
            //var byts = File.ReadAllBytes(filePath);
            //var byts = ReadAllBytesOrFromCopy(filePath);
            //var byts = ReadAllBytesFromCopy(filePath);
            var byts = ReadAllBytesFromReadOnly(filePath);
            var hash = algo.ComputeBytes(byts);
            return hash.ToString().ToLower();
        }


        private static byte[] ReadAllBytesFromReadOnly(string filePath)
        {
            using (var stream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var readr = new StreamReader(stream))
                {
                    var txt = readr.ReadToEnd();
                    return Encoding.UTF8.GetBytes(txt);
                }
            }
        }


        private static byte[] ReadAllBytesOrFromCopy(string filePath)
        {
            byte[] byts;
            try
            {
                byts = File.ReadAllBytes(filePath);
            }
            catch (IOException)
            {
                byts = ReadAllBytesFromCopy(filePath);
            }
            return byts;
        }

        private static byte[] ReadAllBytesFromCopy(string filePath)
        {
            byte[] byts;
            var tmp = filePath.CreateTempCopy();
            byts = File.ReadAllBytes(tmp);
            File.Delete(tmp);
            return byts;
        }

        public static string CreateTempCopy(this string filePath)
        {
            var tmpPath = Path.GetTempFileName();
            File.Copy(filePath, tmpPath, true);
            return tmpPath;
        }


        public static string MakeAbsolute(this string filePath)
        {
            if (Path.IsPathRooted(filePath))
                return filePath;
            else
                return Path.Combine(CurrentExe.GetDirectory(), filePath);
        }


        public static string GetVersion(this string filePath)
            => FileVersionInfo.GetVersionInfo(filePath).FileVersion;
    }
}
