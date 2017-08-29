﻿using System.IO;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public static class FilePathExtensions
    {
        public static string SHA1ForFile(this string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var algo = new HashLib.Crypto.SHA1();
            //var byts = File.ReadAllBytes(filePath);
            var byts = ReadAllBytesOrFromCopy(filePath);
            var hash = algo.ComputeBytes(byts);
            return hash.ToString().ToLower();
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
                var tmp = filePath.CreateTempCopy();
                byts = File.ReadAllBytes(tmp);
                File.Delete(tmp);
            }
            return byts;
        }


        public static string CreateTempCopy(this string filePath)
        {
            var tmpPath = Path.GetTempFileName();
            File.Copy(filePath, tmpPath, true);
            return tmpPath;
        }
    }
}
