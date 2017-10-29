using System.IO;

namespace FreshCopy.Tests.FileFactories
{
    public static class FilePathExtensions
    {
        public static string MakeTempCopy (this string origFilePath, string extension = ".tmp")
        {
            var tmp0 = Path.GetTempFileName();
            File.Delete(tmp0);

            var tmp1 = tmp0 + extension;
            File.Copy(origFilePath, tmp1, true);
            return tmp1;
        }
    }
}
