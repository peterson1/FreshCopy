using System;
using Autofac;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using FreshCopy.Common.API.Configuration;
using System.Collections.ObjectModel;
using FreshCopy.Server.Lib45.ViewModels.SoloFileWatcher;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.ViewModels
{
    public class MainVersionKeeperWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Version Keeper";


        public MainVersionKeeperWindowVM(SignalRServerToggleVM signalRServerToggleVM,
                                         CommonLogListVM commonLogListVM,
                                         VersionKeeperSettings versionKeeperSettings)
        {
            Config       = versionKeeperSettings;
            CommonLogs   = commonLogListVM;
            ServerToggle = signalRServerToggleVM;
            ServerToggle.StartServerCmd.ExecuteIfItCan();
        }


        public VersionKeeperSettings  Config        { get; }
        public SignalRServerToggleVM  ServerToggle  { get; }
        public CommonLogListVM        CommonLogs    { get; }


        public ObservableCollection<SoloFileWatcherVM> WatchList { get; } = new ObservableCollection<SoloFileWatcherVM>();


        public void StartFileWatchers(ILifetimeScope scope)
        {
            foreach (var kv in Config.Files)
            {
                var soloWatchr = scope.Resolve<SoloFileWatcherVM>();
                soloWatchr.SetTarget(kv.Value);
                WatchList.Add(soloWatchr);
                soloWatchr.StartWatchingCmd.ExecuteIfItCan();
            }
        }


        protected override async Task BeforeExitApp()
        {
            StartBeingBusy("Stopping Watchers and Server ...");
            foreach (var soloWatchr in WatchList)
            {
                soloWatchr.StopWatchingCmd.ExecuteIfItCan();
                await Task.Delay(1000 * 1);
            }
            await ServerToggle.StopServerCmd.RunAsync();
        }
    }
}
