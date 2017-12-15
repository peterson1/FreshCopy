using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.ns11.DataStructures;
using FreshCopy.FirebaseUploader.WPF.Configuration;
using FreshCopy.FirebaseUploader.WPF.FilePicker;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FreshCopy.FirebaseUploader.WPF.WebAccess
{
    public class FirebaseIO
    {
        private ConfigLoaderVM _loadr;
        private const string ROOT   = "files";
        private const string SUBKEY = "PublicFileInfo";

        public FirebaseIO(ConfigLoaderVM configLoaderVM)
        {
            _loadr = configLoaderVM;
        }


        internal Task<PublicFileInfo> GetInfo(string fileId)
            => Conn.GetNode<PublicFileInfo>(ROOT, fileId, SUBKEY);


        private FirebaseConnection Conn
            => new FirebaseConnection(_loadr.CurrentCfg.FirebaseCreds);


        internal Task<string> Upload(string filePath)
            => Conn.UploadFile(filePath);


        internal Task UpdateNode(CurrentFileVM vm)
            => Conn.UpdateNode(new PublicFileInfo
            {
                DownloadURL = vm.DownloadURL,
                Version     = vm.LocalVersion,
                SHA1        = vm.LocalSHA1
            },
            ROOT, vm.FileID, SUBKEY);


        internal async Task<bool> VerifyUpload(string url, string sha1)
        {
            var tmp = Path.GetTempFileName();

            using (var wc = new WebClient())
                await wc.DownloadFileTaskAsync(url, tmp);

            await Task.Delay(1000);

            var isOK = tmp.SHA1ForFile() == sha1;
            File.Delete(tmp);

            return isOK;
        }
    }
}
