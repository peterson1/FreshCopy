using System;
using System.IO;

namespace FreshCopy.Tests.FileFactories
{
    class CreateFile
    {
        public static string WithText(string content)
        {
            var tmp = Path.GetTempFileName();
            File.WriteAllText(tmp, content);
            return tmp;
        }

        public static string WithRandomText()
            => WithText(DateTime.Now.ToLongTimeString());


        public static string TempCopy(string fileName)
            => $@"SampleBinaries\{fileName}".MakeTempCopy();
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
