using CommonTools.Lib.fx45.InputTools;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class ExeMenuItems
    {
        public static MenuItem CreateGroup(KeyValuePair<string, string> kvp, string latestVersion)
        {
            var grp = new MenuItem { Header = kvp.Key };
            var exe = kvp.Value;

            grp.Items.Add(CreateLabelItem(kvp.Key));
            grp.Items.Add(CreateLabelItem($"latest: ver.{latestVersion}"));
            grp.Items.Add(CreateLaunchLatestItem(exe));

            grp.Items.Add(new Separator());
            grp.Items.Add(RunningInstancesMenuItems.CreateGroup(exe));
            grp.Items.Add(RunningInstancesMenuItems.CreateCloseAllCmd(exe));

            grp.Items.Add(new Separator());
            grp.Items.Add(OldExeVersionsMenuItems.CreateGroup(exe));

            return grp;
        }


        private static MenuItem CreateLaunchLatestItem(string exePath) => new MenuItem
        {
            Header  = "Launch Latest Version",
            Command = R2Command.Relay(_ => Process.Start(exePath))
        };


        //todo: DRY this up
        private static MenuItem CreateLabelItem(string header) => new MenuItem
        {
            Header    = header,
            IsEnabled = false
        };
    }
}
