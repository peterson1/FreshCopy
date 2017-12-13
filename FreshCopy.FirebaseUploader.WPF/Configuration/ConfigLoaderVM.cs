using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.DataStructures;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FreshCopy.FirebaseUploader.WPF.Configuration
{
    public class ConfigLoaderVM : ViewModelBase
    {
        public ConfigLoaderVM(List<UploaderSettings> cfgsList)
        {
            Configs.SetItems(cfgsList);
            CurrentCfg = Configs.FirstOrDefault();
        }


        public UIList<UploaderSettings>  Configs     { get; } = new UIList<UploaderSettings>();
        public UploaderSettings          CurrentCfg  { get; set; }


        internal static List<UploaderSettings> LoadAll()
        {
            var dir     = CurrentExe.GetDirectory();
            var ext     = "*.cfg";
            var matches = Directory.EnumerateFiles(dir, ext);

            return matches.Select(file =>
            {
                var cfg = JsonFile.Read<UploaderSettings>(file);
                cfg.Filename = Path.GetFileNameWithoutExtension(file);
                return cfg;
            }
            ).ToList();
        }
    }
}
