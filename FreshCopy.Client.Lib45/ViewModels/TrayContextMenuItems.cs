using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.Configuration;
using System;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class TrayContextMenuItems
    {
        private UpdateCheckerSettings   _cfg;
        private IMessageBroadcastClient _client;


        public TrayContextMenuItems(UpdateCheckerSettings updateCheckerSettings,
                                    IMessageBroadcastClient messageBroadcastClient)
        {
            _cfg    = updateCheckerSettings;
            _client = messageBroadcastClient;
        }


        internal void SetItems(ContextMenu rootMnu, MainCheckerWindowVM vm)
        {
            while (rootMnu.Items.Count > 1)
                rootMnu.Items.RemoveAt(1);

            try
            {
                foreach (var exe in _cfg.Executables)
                    rootMnu.Items.Add(ExeMenuItems.CreateGroup(exe));
            }
            catch (Exception ex)
            {
                _client.SendException("Set Menu Items for Exe", ex);
            }

            rootMnu.Items.Add(CreateExitMenuItem(vm));
        }


        private MenuItem CreateExitMenuItem(MainCheckerWindowVM vm) => new MenuItem
        {
            Header  = "Exit",
            Command = vm.ExitCmd
        };
    }
}
