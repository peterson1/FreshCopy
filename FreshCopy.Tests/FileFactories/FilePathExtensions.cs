using System.IO;

namespace FreshCopy.Tests.FileFactories
{
    public static class FilePathExtensions
    {
        public static string MakeTempCopy (this string origFilePath)
        {
            var tmp = Path.GetTempFileName();
            File.Copy(origFilePath, tmp, true);
            return tmp;
        }
    }
}
