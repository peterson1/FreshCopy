using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using FreshCopy.Client.Lib45.TargetUpdaters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class OldExeVersionsMenuItems
    {
        public static MenuItem CreateGroup(string exePath)
        {
            var grp = new MenuItem { Header = "Launch Old Version" };

            foreach (var ver in FindOldVersions(exePath))
                grp.Items.Add(CreateVersionRollbackItem(ver));

            return grp;
        }


        private static MenuItem CreateVersionRollbackItem(string backupExePath) => new MenuItem
        {
            Header  = GetVersionHeader(backupExePath),
            Command = R2Command.Relay(_ => RunOldExe(backupExePath))
        };


        private static void RunOldExe(string backupExePath)
        {
            throw new NotImplementedException();
        }


        private static string GetVersionHeader(string backupExePath)
        {
            var prefx = Path.GetFileName(backupExePath).Substring(0, 17);
            var parsd = DateTime.ParseExact(prefx, BackupKeepingFileUpdater.DATE_FMT, null);
            var d8Prt = parsd.ToString("MMM.d, h:mmtt");
            var verNo = backupExePath.GetVersion();
            return $"{d8Prt} :  ver. {verNo}";
        }


        private static IEnumerable<string> FindOldVersions(string exePath)
        {
            var exeDir = Path.GetDirectoryName(exePath);
            var exeNme = Path.GetFileNameWithoutExtension(exePath);
            var bkpDir = Path.Combine(exeDir, BackupKeepingFileUpdater.BACKUP_DIR);
            var filter = $"*_{exeNme}.{BackupKeepingFileUpdater.BACKUP_EXT}";
            return Directory.EnumerateFiles(bkpDir, filter);
        }
    }
}
