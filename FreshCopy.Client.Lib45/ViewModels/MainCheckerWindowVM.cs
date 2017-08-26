using Autofac;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.BroadcastHandlers;
using FreshCopy.Common.API.Configuration;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class MainCheckerWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Update Checker";

        private IMessageBroadcastListener _client;

        public MainCheckerWindowVM(UpdateCheckerSettings updateCheckerSettings,
                                   IMessageBroadcastListener messageBroadcastListener,
                                   SharedLogListVM commonLogListVM)
        {
            _client    = messageBroadcastListener;
            Config     = updateCheckerSettings;
            CommonLogs = commonLogListVM;

            _client.BroadcastReceived += (s, e) 
                => CommonLogs.Add($"[{e.Key}]  {e.Value}");
        }


        public UpdateCheckerSettings  Config       { get; }
        public SharedLogListVM        CommonLogs   { get; }


        public ObservableCollection<IBroadcastHandler> Listeners { get; } = new ObservableCollection<IBroadcastHandler>();


        public void StartBroadcastHandlers(ILifetimeScope scope)
        {
            Config.BinaryFiles.Add(CheckerRelease.FileKey, 
                                   CurrentExe.GetFullPath());

            foreach (var kv in Config.BinaryFiles)
                StartNewHandler<BinaryFileBroadcastHandlerVM>(scope, kv);

            foreach (var kv in Config.AppendOnlyDBs)
                StartNewHandler<AppendOnlyDbBroadcastHandlerVM>(scope, kv);
        }


        private void StartNewHandler<T>(ILifetimeScope scope, System.Collections.Generic.KeyValuePair<string, string> kv)
            where T : IBroadcastHandler
        {
            var listnr = scope.Resolve<T>();
            listnr.SetTargetFile(kv.Key, kv.Value);
            Listeners.Add(listnr);
        }


        protected override async Task OnWindowLoad()
        {
            await _client.Connect();
        }


        protected override async Task OnWindowClose()
        {
            StartBeingBusy("Disconnecting client ...");
            _client.Disconnect();
            await Task.Delay(1000);
        }
    }
}
