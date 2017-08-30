using System;
using System.IO;

namespace CommonTools.Lib.fx45.LiteDbTools
{
    public abstract class LocalUserKeyValueStore1 : LiteDbKeyValueStore1
    {
        public LocalUserKeyValueStore1(string fileName, string subDir = "CommonTools.Lib") 
            : base(FindLocalUserPath(fileName, subDir))
        {
        }


        private static string FindLocalUserPath(string fileName, string subDir)
        {
            var special = Environment.SpecialFolder.LocalApplicationData;
            var baseDir = Environment.GetFolderPath(special);
            var fullDir = Path.Combine(baseDir, subDir);
            Directory.CreateDirectory(fullDir);
            return Path.Combine(fullDir, fileName);
        }
    }
}
