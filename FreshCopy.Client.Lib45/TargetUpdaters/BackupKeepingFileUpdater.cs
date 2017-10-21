using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.HubClients;
using System;
using System.IO;

namespace FreshCopy.Client.Lib45.TargetUpdaters
{
    public class BackupKeepingFileUpdater : BinaryFileUpdater1
    {
        public const string BACKUP_DIR = "FC_Backups";
        public const string BACKUP_EXT = "backup";
        public const string DATE_FMT   = "yyyy-MM-dd_hhmmss";

        public BackupKeepingFileUpdater(IVersionKeeperClient versionKeeperClient, IMessageBroadcastClient messageBroadcastListener) : base(versionKeeperClient, messageBroadcastListener)
        {
        }


        protected override void DecodeB64ToDisk(string b64)
        {
            var backupPath = CreateBackupPath(_filePath);

            if (File.Exists(_filePath))
                File.Move(_filePath, backupPath);

            base.DecodeB64ToDisk(b64);
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
    }
}
