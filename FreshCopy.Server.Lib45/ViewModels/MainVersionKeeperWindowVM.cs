using Autofac;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.FileWatchers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using Microsoft.AspNet.SignalR;
using FreshCopy.Server.Lib45.SignalRHubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Hubs;

namespace FreshCopy.Server.Lib45.ViewModels
{
    public class MainVersionKeeperWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Version Keeper";


        public MainVersionKeeperWindowVM(SignalRServerToggleVM signalRServerToggleVM,
                                         CurrentHubClientsVM currentHubClientsVM,
                                         CommonLogListVM commonLogListVM,
                                         VersionKeeperSettings versionKeeperSettings)
        {
            Config       = versionKeeperSettings;
            Clients      = currentHubClientsVM;
            CommonLogs   = commonLogListVM;
            ServerToggle = signalRServerToggleVM;
            ServerToggle.StartServerCmd.ExecuteIfItCan();
            TestSend1Cmd = R2Command.Async(TestSend1);
            TestSend2Cmd = R2Command.Async(TestSend2);
        }


        private async Task TestSend1()
        {
            try
            {
                var connMgr = GlobalHost.DependencyResolver.Resolve<IConnectionManager>();
                //var connMgr = GlobalHost.ConnectionManager;

                var hubCtx = connMgr.GetHubContext<VersionKeeperHub1>();
                //var hubCtx = connMgr.GetHubContext("VersionKeeperHub");

                var proxy = hubCtx.Clients.All as IClientProxy;
                //var proxy = hubCtx.Clients.Client(Clients.List[0]) as IClientProxy;
                await proxy.Invoke("SendMessage", "hi");

                //await hubCtx.Clients.All.SendMessage("hi");

                CommonLogs.Add("Test 1 sent.");
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
                var ctx = GlobalHost.ConnectionManager.GetHubContext<VersionKeeperHub1>();
                await ctx.Clients.All.SendMessage("from server t2");
                CommonLogs.Add("send 2");
            }
            catch (Exception ex)
            {
                CommonLogs.Add(ex);
            }
        }


        public CurrentHubClientsVM    Clients       { get; }
        public VersionKeeperSettings  Config        { get; }
        public SignalRServerToggleVM  ServerToggle  { get; }
        public CommonLogListVM        CommonLogs    { get; }


        public IR2Command TestSend1Cmd { get; }
        public IR2Command TestSend2Cmd { get; }


        public ObservableCollection<BinaryFileWatcherVM> WatchList { get; } = new ObservableCollection<BinaryFileWatcherVM>();


        public void StartFileWatchers(ILifetimeScope scope)
        {
            foreach (var kv in Config.BinaryFiles)
            {
                var soloWatchr = scope.Resolve<BinaryFileWatcherVM>();
                soloWatchr.SetTargetFile(kv.Key, kv.Value);
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
