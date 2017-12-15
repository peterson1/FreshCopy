using CommonTools.Lib.fx45.ViewModelTools;
using FreshCopy.FirebaseUploader.WPF.Configuration;
using FreshCopy.FirebaseUploader.WPF.FilePicker;
using System.Linq;

namespace FreshCopy.FirebaseUploader.WPF
{
    public class MainWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "FC Firebase Uploader";


        public MainWindowVM(ConfigLoaderVM configLoaderVM,
                            FilePickerVM filePickerVM)
        {
            ConfigLoader = configLoaderVM;
            FilePicker   = filePickerVM;
        }


        public ConfigLoaderVM  ConfigLoader  { get; }
        public FilePickerVM    FilePicker    { get; }


        protected override void OnWindowLoad()
        {
            ConfigLoader.CurrentCfg = ConfigLoader.Configs.FirstOrDefault();
        }
    }
}
