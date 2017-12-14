using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using FreshCopy.FirebaseUploader.WPF.WebAccess;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FreshCopy.FirebaseUploader.WPF.FilePicker
{
    public class CurrentFileVM : ViewModelBase
    {
        private FirebaseIO _fBase;

        public CurrentFileVM(FirebaseIO firebaseIO)
        {
            _fBase = firebaseIO;
            UploadCmd = R2Command.Async(DoUpload, _ => CanUpload(), "Upload");
        }


        public string      Key            { get; private set; }
        public string      Path           { get; private set; }
        public bool        Found          { get; private set; }
        public string      LocalSHA1      { get; private set; }
        public string      LocalVersion   { get; private set; }
        public string      RemoteSHA1     { get; private set; }
        public string      RemoteVersion  { get; private set; }
        public bool        IsSame         { get; private set; }
        public IR2Command  UploadCmd      { get; }


        internal async Task LoadFile(string fileKey, string filePath)
        {
            if (IsBusy) return;
            Key           = fileKey;
            Path          = filePath;
            Found         = File.Exists(Path);
            LocalSHA1     = Found ? Path.SHA1ForFile() : "--";
            LocalVersion  = Found ? Path.GetVersion()  : "--";
            RemoteSHA1    = "--";
            RemoteVersion = "--";
            IsSame        = true;
            if (Found) await CompareWithRemote();
        }


        private async Task CompareWithRemote()
        {
            StartBeingBusy("Comparing with remote file ...");
            await GetRemoteFileInfo();
            IsSame = LocalSHA1 == RemoteSHA1;
            CommandManager.InvalidateRequerySuggested();
            StopBeingBusy();
        }


        private async Task GetRemoteFileInfo()
        {
            var node = await _fBase.GetInfo(Key);
            if (node == null) return;
            RemoteSHA1    = node.SHA1;
            RemoteVersion = node.Version;
        }


        private bool CanUpload()
        {
            if (IsBusy) return false;
            if (IsSame) return false;
            return true;
        }


        private async Task DoUpload()
        {
            StartBeingBusy("Uploading ...");
            await Task.Delay(1000);
            StopBeingBusy();
        }
    }
}
