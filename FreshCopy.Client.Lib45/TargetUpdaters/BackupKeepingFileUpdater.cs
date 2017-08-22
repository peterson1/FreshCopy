using FreshCopy.Common.API.HubClients;
using System;
using System.IO;

namespace FreshCopy.Client.Lib45.TargetUpdaters
{
    public class BackupKeepingFileUpdater : BinaryFileUpdater1
    {
        private const string BACKUP_DIR = "FC_Backups";

        public BackupKeepingFileUpdater(IVersionKeeperClient versionKeeperClient) : base(versionKeeperClient)
        {
        }


        protected override void DecodeB64ToDisk(string b64)
        {
            var backupPath = CreateBackupPath(_filePath);
            File.Move(_filePath, backupPath);

            base.DecodeB64ToDisk(b64);
        }


        private string CreateBackupPath(string targetPath)
        {
            var dir = GetOrCreateBackupDir(targetPath);
            var pre = DateTime.Now.ToString("yyyy-MM-dd_hhmmss");
            var nme = Path.GetFileNameWithoutExtension(targetPath);
            var ext = "backup";
            return Path.Combine(dir, $"{pre}_{nme}.{ext}");
        }


        private string GetOrCreateBackupDir(string targetPath)
        {
            var baseDir = Path.GetDirectoryName(targetPath);
            var bkpDir  = Path.Combine(baseDir, BACKUP_DIR);
            Directory.CreateDirectory(bkpDir);
            return bkpDir;
        }
    }
}
