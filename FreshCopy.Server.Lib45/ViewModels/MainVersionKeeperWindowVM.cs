using Autofac;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.SignalrTools;
using CommonTools.Lib.fx45.UserControls.AppUpdateNotifiers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.FileWatchers;
using FreshCopy.Server.Lib45.HubClientStates;
using FreshCopy.Server.Lib45.SignalRHubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.ViewModels
{
    public class MainVersionKeeperWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Version Keeper";

        private ClonedCopyExeUpdater _cloneUpdatr;


        public MainVersionKeeperWindowVM(SignalrServerToggleVM signalRServerToggleVM,
                                         CurrentHubClientsVM currentHubClientsVM,
                                         SharedLogListVM commonLogListVM,
                                         VersionKeeperSettings versionKeeperSettings,
                                         ClonedCopyExeUpdater clonedCopyExeUpdater,
                                         AppUpdateNotifierVM appUpdateNotifierVM)
        {
            Config       = versionKeeperSettings;
            Clients      = currentHubClientsVM;
            CommonLogs   = commonLogListVM;
            Updater      = appUpdateNotifierVM;
            _cloneUpdatr = clonedCopyExeUpdater;

            ServerToggle = signalRServerToggleVM;
            ServerToggle.StartServerCmd.ExecuteIfItCan();

            TestSend1Cmd = R2Command.Async(TestSend1, null, "Request States");
            TestSend2Cmd = R2Command.Async(TestSend2, null, "Test Broadcast");
        }


        public CurrentHubClientsVM    Clients       { get; }
        public VersionKeeperSettings  Config        { get; }
        public SignalrServerToggleVM  ServerToggle  { get; }
        public SharedLogListVM        CommonLogs    { get; }
        public AppUpdateNotifierVM    Updater       { get; }


        public IR2Command TestSend1Cmd { get; }
        public IR2Command TestSend2Cmd { get; }


        public ObservableCollection<FileWatcherVMBase> WatchList { get; } = new ObservableCollection<FileWatcherVMBase>();


        public void StartFileWatchers(ILifetimeScope scope)
        {
            foreach (var kv in Config.BinaryFiles)
                StartNewWatcher<BinaryFileWatcherVM>(scope, kv);

            foreach (var kv in Config.AppendOnlyDBs)
                StartNewWatcher<AppendOnlyDbWatcherVM>(scope, kv);

            Updater.StartChecking();

            if (!Config.MasterCopy.IsBlank())
                _cloneUpdatr.StartWatching(Config.MasterCopy);
        }


        private void StartNewWatcher<T>(ILifetimeScope scope, System.Collections.Generic.KeyValuePair<string, string> kv)
            where T : FileWatcherVMBase
        {
            var soloWatchr = scope.Resolve<T>();
            soloWatchr.SetTargetFile(kv.Key, kv.Value);
            WatchList.Add(soloWatchr);
            soloWatchr.StartWatchingCmd.ExecuteIfItCan();
        }


        protected override async Task OnWindowCloseAsync()
        {
            StartBeingBusy("Stopping Watchers and Server ...");
            foreach (var soloWatchr in WatchList)
            {
                soloWatchr.StopWatchingCmd.ExecuteIfItCan();
                await Task.Delay(1000 * 1);
            }
            await ServerToggle.StopServerCmd.RunAsync();
        }


        private async Task TestSend1()
        {
            try
            {
                //await MessageBroadcast.RequestClientStates();
                await MBHub.All.RequestClientState();
                CommonLogs.Add("Sent request for client states.");
            }
            catch (Exception ex)
            {
                CommonLogs.Add(ex);
            }
        }


        private async Task TestSend2()
        {
            try
            {
                //await MessageBroadcast.ToAllClients("subj here", "test MessageBroadcast.ToAllClients");
                await MBHub.BroadcastToAll("subj here", "test MessageBroadcast.ToAllClients");
                CommonLogs.Add("MessageBroadcast sent");
            }
            catch (Exception ex)
            {
                CommonLogs.Add(ex);
            }
        }
    }
}
