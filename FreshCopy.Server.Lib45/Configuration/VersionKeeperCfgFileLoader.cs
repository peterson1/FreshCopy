using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using System.Collections.Generic;
using System.IO;

namespace FreshCopy.Server.Lib45.Configuration
{
    public class VersionKeeperCfgFile
    {
        public const string FILE_NAME = "FC.VersionKeeper.cfg";


        public static VersionKeeperSettings LoadOrDefault()
        {
            VersionKeeperSettings cfg;
            try
            {
                cfg = JsonFile.Read<VersionKeeperSettings>(FILE_NAME);
            }
            catch (FileNotFoundException)
            {
                return WriteDefaultSettingsFile();
            }
            SetDefaults(ref cfg);
            return cfg;
        }


        private static void SetDefaults(ref VersionKeeperSettings cfg)
        {
            if (cfg.BinaryFiles   == null) cfg.BinaryFiles   = new Dictionary<string, string>();
            if (cfg.AppendOnlyDBs == null) cfg.AppendOnlyDBs = new Dictionary<string, string>();
        }


        private static VersionKeeperSettings WriteDefaultSettingsFile()
        {
            var cfg = VersionKeeperSettings.CreateDefault();
            JsonFile.Write(cfg, FILE_NAME);
            return cfg;
        }
    }
}
