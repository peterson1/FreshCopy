using CommonTools.Lib.fx45.UIExtensions;
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

            grp.Items.AddDisabledItem(kvp.Key);
            grp.Items.AddDisabledItem($"latest: ver.{latestVersion}");
            grp.Items.AddCommandItem("Launch Latest Version", 
                                    _ => Process.Start(exe));

            grp.Items.Add(new Separator());
            grp.Items.Add(RunningInstancesMenuItems.CreateGroup(exe));
            grp.Items.Add(RunningInstancesMenuItems.CreateCloseAllCmd(exe));

            grp.Items.Add(new Separator());
            grp.Items.Add(OldExeVersionsMenuItems.CreateGroup(exe));

            return grp;
        }
    }
}
