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
            try
            {
                return JsonFile.Read<UpdateCheckerSettings>(FILE_NAME);
            }
            catch (FileNotFoundException)
            {
                return WriteDefaultSettingsFile();
            }
        }


        private static UpdateCheckerSettings WriteDefaultSettingsFile()
        {
            var cfg = UpdateCheckerSettings.CreateDefault();
            JsonFile.Write(cfg, FILE_NAME);
            return cfg;
        }
    }
}
