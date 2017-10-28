using System;
using System.IO;

namespace FreshCopy.Tests.FileFactories
{
    class CreateFile
    {
        public static string WithRandomText()
        {
            var tmp = Path.GetTempFileName();
            File.WriteAllText(tmp, DateTime.Now.ToLongTimeString());
            return tmp;
        }
    }


    class CreateDir
    {
        public static string InTemp()
        {
            var path = Path.Combine(Path.GetTempPath(), RandomText());
            Directory.CreateDirectory(path);
            return path;
        }


        private static string RandomText()
            => Path.GetRandomFileName().Replace(".", "");
    }
}
