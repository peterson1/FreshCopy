using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using System.IO;

namespace FreshCopy.Server.Lib45.Configuration
{
    public class VersionKeeperCfgFile
    {
        public const string FILE_NAME = "FC.VersionKeeper.cfg";


        public static VersionKeeperSettings LoadOrDefault()
        {
            try
            {
                return JsonFile.Read<VersionKeeperSettings>(FILE_NAME);
            }
            catch (FileNotFoundException)
            {
                return WriteDefaultSettingsFile();
            }
        }


        private static VersionKeeperSettings WriteDefaultSettingsFile()
        {
            var cfg = VersionKeeperSettings.CreateDefault();
            JsonFile.Write(cfg, FILE_NAME);
            return cfg;
        }
    }
}
