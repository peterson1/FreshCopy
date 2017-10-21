using FreshCopy.Common.API.Configuration;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class TrayContextMenuItems
    {
        private UpdateCheckerSettings _cfg;


        public TrayContextMenuItems(UpdateCheckerSettings updateCheckerSettings)
        {
            _cfg      = updateCheckerSettings;
        }


        internal void SetItems(ContextMenu rootMnu, MainCheckerWindowVM vm)
        {
            while (rootMnu.Items.Count > 1)
                rootMnu.Items.RemoveAt(1);

            foreach (var exe in _cfg.Executables)
                rootMnu.Items.Add(ExeMenuItems.CreateGroup(exe));

            rootMnu.Items.Add(CreateExitMenuItem(vm));
        }


        private MenuItem CreateExitMenuItem(MainCheckerWindowVM vm)
        {
            var itm = new MenuItem();
            itm.Header = "Exit";
            itm.Command = vm.ExitCmd;

            return itm;
        }
    }
}
