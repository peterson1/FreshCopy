using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using System.IO;

namespace FreshCopy.Client.Lib45.Configuration
{
    public class UpdateCheckerCfgFile
    {
        public const string FILE_NAME = "FC.UpdateChecker.cfg";


        public static UpdateCheckerSettings LoadOrDefault()
        {
            UpdateCheckerSettings cfg;
            try
            {
                cfg = JsonFile.Read<UpdateCheckerSettings>(FILE_NAME);
            }
            catch (FileNotFoundException)
            {
                return WriteDefaultSettingsFile();
            }
            SetDefaults(ref cfg);
            return cfg;
        }


        private static void SetDefaults(ref UpdateCheckerSettings cfg)
        {
            if (!cfg.UpdateSelf.HasValue) cfg.UpdateSelf = true;
        }


        private static UpdateCheckerSettings WriteDefaultSettingsFile()
        {
            var cfg = UpdateCheckerSettings.CreateDefault();
            JsonFile.Write(cfg, FILE_NAME);
            return cfg;
        }
    }
}
