using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.UIExtensions;
using CommonTools.Lib.ns11.StringTools;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class RunningInstancesMenuItems
    {
        public static MenuItem CreateGroup(string exePath)
        {
            var procs = FindInstances(exePath);
            var headr = $"Running instances :  {procs.Length}";
            var group = new MenuItem { Header = headr };

            foreach (var proc in procs)
                group.Items.Add(CreateProcessMenuItem(proc));

            return group;
        }

        public static MenuItem CreateCloseAllCmd(string exePath) => new MenuItem
        {
            Header = "Close All Instances",
            Command = R2Command.Relay(
                    _ => CloseAllInstances(exePath),
                    _ => CountInstances(exePath) > 0)
        };


        private static void CloseAllInstances(string exePath)
        {
            var exeNme = Path.GetFileName(exePath);
            var messge = $"All running instances of “{exeNme}” will be closed."
                 + L.F + "You may lose unsaved changes."
                 + L.F + "Are you sure you want to proceed?";

            Alert.Confirm($"Closing ALL instances of “{exeNme}” ...", messge, ()
                => KillProcess.ByName(exeNme, true));
        }


        private static MenuItem CreateProcessMenuItem(Process proc)
        {
            var ver = proc.MainModule.FileVersionInfo.FileVersion;
            var hdr = $"[pid:{proc.Id}]  ver.{ver}";
            var grp = new MenuItem { Header = hdr };

            grp.Items.AddDisabledItem(
                $"Memory used:  {GetMemoryUsageMB(proc):N0} MB");

            grp.Items.AddCommandItem("End this Process",
                                 _ => ConfirmKill(proc));
            return grp;
        }


        private static long GetMemoryUsageMB(Process proc)
            => (proc.PrivateMemorySize64 / 1024) / 1024;


        private static void ConfirmKill(Process proc)
        {
            var exe = Path.GetFileNameWithoutExtension
                            (proc.MainModule.FileName);
            var msg = $"“{exe}” will be closed."
              + L.F + "You may lose unsaved changes."
              + L.F + "Are you sure you want to proceed?";

            Alert.Confirm($"Closing “{exe}” ...", msg, () 
                => proc.Kill());
        }


        private static Process[] FindInstances(string exePath)
            => Process.GetProcessesByName(
                Path.GetFileNameWithoutExtension(exePath));


        private static int CountInstances(string exePath)
            => FindInstances(exePath).Length;
    }
}
