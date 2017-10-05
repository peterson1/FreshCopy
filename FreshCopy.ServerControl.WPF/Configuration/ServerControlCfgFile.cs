using CommonTools.Lib.fx45.FileSystemTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCopy.ServerControl.WPF.Configuration
{
    class ServerControlCfgFile
    {
        const string FILE_NAME = "FC.ServerControl.cfg";


        internal static ServerControlSettings LoadOrDefault()
        {
            ServerControlSettings cfg;
            try
            {
                cfg = JsonFile.Read<ServerControlSettings>(FILE_NAME);
            }
            catch (FileNotFoundException)
            {
                return WriteDefaultSettingsFile();
            }
            SetDefaults(ref cfg);
            return cfg;
        }


        private static void SetDefaults(ref ServerControlSettings cfg)
        {
            //if (!cfg.UpdateSelf.HasValue) cfg.UpdateSelf = true;
            //if (!cfg.CanExitApp.HasValue) cfg.CanExitApp = false;
        }


        private static ServerControlSettings WriteDefaultSettingsFile()
        {
            var cfg = ServerControlSettings.CreateDefault();
            JsonFile.Write(cfg, FILE_NAME);
            return cfg;
        }
    }
}
