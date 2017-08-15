using System.IO;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public static class FilePathExtensions
    {
        public static string SHA1ForFile(this string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var algo = new HashLib.Crypto.SHA1();
            var byts = File.ReadAllBytes(filePath);
            var hash = algo.ComputeBytes(byts);
            return hash.ToString().ToLower();
        }
    }
}
