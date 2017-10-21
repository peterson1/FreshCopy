using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ThreadTools;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class ExeMenuItems
    {


        public static MenuItem CreateGroup(KeyValuePair<string, string> kvp)
        {
            var menuItm = new MenuItem { Header = kvp.Key };
            var exePath = kvp.Value;

            menuItm.Items.Add(CreateInstanceCountItem(exePath));
            menuItm.Items.Add(CreateKillExeItem(exePath, false));
            menuItm.Items.Add(CreateKillExeItem(exePath, true));

            return menuItm;
        }


        private static MenuItem CreateKillExeItem(string exePath, bool killForcefully)
        {
            var hdr  = killForcefully ? "Kill Process Forcefully" 
                                      : "Exit Task Gracefully";
            var item = new MenuItem { Header = hdr };
            var nme  = Path.GetFileName(exePath);

            item.Command = R2Command.Relay(_ 
                => KillProcess.ByName(nme, killForcefully));

            return item;
        }


        private static MenuItem CreateInstanceCountItem(string exePath)
            => new MenuItem
            {
                Header = $"Running instances: {CountInstances(exePath)}",
                IsEnabled = false,
            };


        private static int CountInstances(string exePath)
        {
            var nme   = Path.GetFileNameWithoutExtension(exePath);
            var procs = Process.GetProcessesByName(nme);
            return procs.Length;
        }
    }
}
