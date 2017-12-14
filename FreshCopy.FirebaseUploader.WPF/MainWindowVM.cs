using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.InputTools;
using FreshCopy.FirebaseUploader.WPF.Configuration;
using FreshCopy.FirebaseUploader.WPF.FilePicker;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace FreshCopy.FirebaseUploader.WPF
{
    public class MainWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "FC Firebase Uploader";

        //private UploaderSettings _cfg;

        public MainWindowVM(ConfigLoaderVM configLoaderVM,
                            FilePickerVM filePickerVM)
        {
            ConfigLoader = configLoaderVM;
            FilePicker   = filePickerVM;
            //UploadCmd = R2Command.Async(DoUpload, _ => !IsBusy, "Upload");
            //UploadCmd.DisableWhenDone = true;
        }


        public ConfigLoaderVM  ConfigLoader  { get; }
        public FilePickerVM    FilePicker    { get; }

        //public string       FileID        { get; private set; }
        //public string       LocalPath     { get; private set; }
        //public bool         IsFound       { get; private set; }
        //public string       FileVersion   { get; set; }
        //public string       FileSHA1      { get; set; }
        //public string       DownloadURL   { get; set; }
        //public IR2Command   UploadCmd     { get; }
        //public IR2Command   TestFileCmd   { get; }


        protected override void OnWindowLoad()
        {
            //_cfg = JsonFile.Read<UploaderSettings>("FC.FirebaseUploader.cfg");
            //FileID      = _cfg.PublicFiles.ElementAt(0).Key;
            //LocalPath   = _cfg.PublicFiles.ElementAt(0).Value;
            //IsFound     = File.Exists(LocalPath);
            //FileSHA1    = LocalPath.SHA1ForFile();
            //FileVersion = LocalPath.GetVersion();

            ConfigLoader.CurrentCfg = ConfigLoader.Configs.FirstOrDefault();
        }


        //private async Task DoUpload()
        //{
        //    StartBeingBusy("Uploading ...");

        //    var conn = new FirebaseConnection(_cfg.FirebaseCreds);
        //    DownloadURL = await conn.UploadFile(LocalPath);

        //    var ok = await VerifyUploadedFile();
        //    if (!ok) return;

        //    await conn.UpdateNode(new PublicFileInfo
        //    {
        //        DownloadURL = DownloadURL,
        //        Version     = FileVersion,
        //        SHA1        = FileSHA1
        //    }, 
        //    "files", FileID, "PublicFileInfo");

        //    StopBeingBusy();

        //}


        //private async Task<bool> VerifyUploadedFile()
        //{
        //    StartBeingBusy("Verifying uploaded file ...");
        //    var tmp = Path.GetTempFileName();

        //    using (var wc = new WebClient())
        //        await wc.DownloadFileTaskAsync(DownloadURL, tmp);

        //    await Task.Delay(1000);

        //    var isOk = tmp.SHA1ForFile() == FileSHA1;

        //    if (isOk)
        //        MessageBox.Show("File maintained its integrity", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //    else
        //        MessageBox.Show("File got CORRUPTED", "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);

        //    File.Delete(tmp);
        //    StopBeingBusy();

        //    return isOk;
        //}
    }
}
