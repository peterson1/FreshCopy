using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using FreshCopy.FirebaseUploader.WPF.WebAccess;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FreshCopy.FirebaseUploader.WPF.FilePicker
{
    public class CurrentFileVM : ViewModelBase
    {
        private FirebaseIO _fBase;

        public CurrentFileVM(FirebaseIO firebaseIO)
        {
            _fBase = firebaseIO;
            UploadCmd  = R2Command.Async(DoUpload, _ => CanUpload(), "Upload");
            RefreshCmd = R2Command.Async(CompareWithRemote, _ => !IsBusy, "Refresh");
        }


        public string      FileID         { get; private set; }
        public string      LocalPath      { get; private set; }
        public bool        Found          { get; private set; }
        public string      LocalSHA1      { get; private set; }
        public string      LocalVersion   { get; private set; }
        public string      RemoteSHA1     { get; private set; }
        public string      RemoteVersion  { get; private set; }
        public string      DownloadURL    { get; private set; }
        public bool        IsSame         { get; private set; }
        public IR2Command  UploadCmd      { get; }
        public IR2Command  RefreshCmd     { get; }


        internal async Task LoadFile(string fileKey, string filePath)
        {
            FileID        = fileKey;
            LocalPath     = filePath;
            Found         = File.Exists(LocalPath);
            LocalSHA1     = Found ? LocalPath.SHA1ForFile() : "--";
            LocalVersion  = Found ? LocalPath.GetVersion()  : "--";
            RemoteSHA1    = "--";
            RemoteVersion = "--";
            DownloadURL   = "--";
            IsSame        = true;
            await RefreshCmd.RunAsync();
        }


        private async Task CompareWithRemote()
        {
            if (!Found) return;
            StartBeingBusy("Comparing with remote file ...");
            await GetRemoteFileInfo();
            IsSame = LocalSHA1 == RemoteSHA1;
            CommandManager.InvalidateRequerySuggested();
            StopBeingBusy();
        }


        private async Task GetRemoteFileInfo()
        {
            var node = await _fBase.GetInfo(FileID);
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
            var fnme = System.IO.Path.GetFileName(LocalPath);
            StartBeingBusy($"Uploading “{fnme}” ...");

            DownloadURL = await _fBase.Upload(LocalPath);
            var ok = await VerifyUploadedFile(DownloadURL);
            if (!ok) return;

            await _fBase.UpdateNode(this);
            await RefreshCmd.RunAsync();
        }


        private async Task<bool> VerifyUploadedFile(string url)
        {
            StartBeingBusy("Verifying uploaded file ...");

            var isOk = await _fBase.VerifyUpload(url, LocalSHA1);
            if (isOk)
                MessageBox.Show("File maintained its integrity", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("File got CORRUPTED", "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);

            StopBeingBusy();
            return isOk;
        }

    }
}
