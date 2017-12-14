using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.FirebaseUploader.WPF.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FreshCopy.FirebaseUploader.WPF.FilePicker
{
    public class FilePickerVM : ViewModelBase
    {
        private Dictionary<string, string> _dict;
        private ConfigLoaderVM     _loadr;

        public FilePickerVM(ConfigLoaderVM configLoadr,
                            CurrentFileVM currentFileVM)
        {
            CurrentFile = currentFileVM;
            _loadr      = configLoadr;
            _loadr.PropertyChanged += ConfigLoadr_PropertyChanged;
            this  .PropertyChanged += FilePickerVM_PropertyChanged;
        }


        public UIList<string>  FileKeys    { get; } = new UIList<string>();
        public CurrentFileVM   CurrentFile { get; }

        public string          CurrentKey  { get; set; }
        public string          CurrentPath => GetCurrentPath();


        private string GetCurrentPath()
        {
            if (CurrentKey.IsBlank()) return "";
            if (_dict.TryGetValue(CurrentKey, out string path))
                return path;
            else
                return "";
        }


        private void ConfigLoadr_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ConfigLoaderVM.CurrentCfg))
            {
                _dict = _loadr.CurrentCfg.PublicFiles;
                FileKeys.SetItems(_dict.Select(_ => _.Key));
                CurrentKey = FileKeys.FirstOrDefault();
            }
        }


        private async void FilePickerVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentKey))
                await CurrentFile.LoadFile(CurrentKey, CurrentPath);
        }
    }
}
