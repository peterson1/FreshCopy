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


        protected override void DecodeB64ToDisk(string b64, string targetPath)
        {
            var backupPath = CreateBackupPath(targetPath);
            File.Move(targetPath, backupPath);

            base.DecodeB64ToDisk(b64, targetPath);
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
            //Log($"Backup: {bkpDir}");
            return bkpDir;
        }
    }
}
