using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.LoggingTools;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public class NewVersionWatcher : IDisposable
    {
        private FirebaseConnection  _conn;
        private AgentStateUpdater   _agtState;

        private const string ROOTKEY    = "files";
        private const string SUBKEY     = "PublicFileInfo";
        private const string BACKUP_DIR = "FC_Backups";
        private const string BACKUP_EXT = "backup";
        private const string DATE_FMT   = "yyyy-MM-dd_hhmmss";


        public NewVersionWatcher(FirebaseConnection firebaseConnection,
                                 AgentStateUpdater agentStateUpdater)
        {
            _conn     = firebaseConnection;
            _agtState = agentStateUpdater;
        }


        public async Task<bool> NewVersionInstalled(string fileId)
        {
            //var rel = @"C:\Users\Pete\Code\FreshCopy\FreshCopy.UpdateChecker.WPF\bin\Release\FC.UpdateChecker.exe";
            //var rh = rel.SHA1ForFile();
            //var rv = rel.GetVersion();

            var currExe  = CurrentExe.GetFullPath();
            var loccSHA1 = currExe.SHA1ForFile();
            await _agtState.SetState("Checking for updates", loccSHA1);

            var node = await _conn.GetNode<PublicFileInfo>(ROOTKEY, fileId, SUBKEY);
            if (node == null) return false;
            if (loccSHA1 == node.SHA1)
            {
                await _agtState.SetRunningTask("Already running the latest version.");
                return false;
            }

            await _agtState.SetRunningTask("Downloading newer version");
            var tmp = await DownloadToTemp(node);
            var bkp = CreateBackupPath(currExe);
            File.Move(currExe, bkp);
            File.Move(tmp, currExe);

            return true;
        }



        private string CreateBackupPath(string targetPath)
        {
            var dir = GetOrCreateBackupDir(targetPath);
            var pre = DateTime.Now.ToString(DATE_FMT);
            var nme = Path.GetFileNameWithoutExtension(targetPath);
            return Path.Combine(dir, $"{pre}_{nme}.{BACKUP_EXT}");
        }


        private string GetOrCreateBackupDir(string targetPath)
        {
            var baseDir = Path.GetDirectoryName(targetPath);
            var bkpDir  = Path.Combine(baseDir, BACKUP_DIR);
            Directory.CreateDirectory(bkpDir);
            return bkpDir;
        }



        private async Task<string> DownloadToTemp(PublicFileInfo file)
        {
            string tmp = string.Empty;
            string hash = string.Empty;
            do
            {
                tmp = Path.GetTempFileName();
                using (var wc = new WebClient())
                {
                    try   { await wc.DownloadFileTaskAsync(file.DownloadURL, tmp); }
                    catch (Exception ex){ await Loggly.Post(ex); }
                }
                await Task.Delay(1000 * 2);
                hash = tmp.SHA1ForFile();
            } while (hash != file.SHA1);

            return tmp;
        }


        private string SHA1Url(string fileId)
            => $"{ROOTKEY}/{fileId}/{nameof(PublicFileInfo.SHA1)}";


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                _agtState?.Dispose();
                _agtState = null;
                _conn?.Dispose();
                _conn  = null;
            }
            disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
