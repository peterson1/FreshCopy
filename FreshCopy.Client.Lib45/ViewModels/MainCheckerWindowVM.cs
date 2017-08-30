using Autofac;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
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


        //public void StartBroadcastHandlers(ILifetimeScope scope)
        //{
        //    Config.BinaryFiles.Add(CheckerRelease.FileKey, 
        //                           CurrentExe.GetFullPath());

        //    foreach (var kv in Config.BinaryFiles)
        //        StartNewHandler<BinaryFileBroadcastHandlerVM>(scope, kv);

        //    foreach (var kv in Config.AppendOnlyDBs)
        //        StartNewHandler<AppendOnlyDbBroadcastHandlerVM>(scope, kv);
        //}


        public async Task StartBroadcastHandlers(ILifetimeScope scope)
        {
            await StartNewHandler<BinaryFileBroadcastHandlerVM>(scope, 
                CheckerRelease.FileKey, CurrentExe.GetFullPath());

            foreach (var kv in Config.BinaryFiles)
                await StartNewHandler<BinaryFileBroadcastHandlerVM>(scope, kv.Key, kv.Value);

            foreach (var kv in Config.AppendOnlyDBs)
                await StartNewHandler<AppendOnlyDbBroadcastHandlerVM>(scope, kv.Key, kv.Value);
        }


        //private IBroadcastHandler CreateSelfListener(ILifetimeScope scope)
        //    => StartNewHandler<BinaryFileBroadcastHandlerVM>(scope, 
        //        CheckerRelease.FileKey, CurrentExe.GetFullPath(), false);


        private async Task<T> StartNewHandler<T>(ILifetimeScope scope, string fileKey, string filePath)
            where T : IBroadcastHandler
        {
            var listnr = scope.Resolve<T>();
            listnr.SetTargetFile(fileKey, filePath);
            UIThread.Run(() => Listeners.Add(listnr));

            await listnr.CheckThenSetHandler();

            return listnr;
        }


        protected override async Task OnWindowLoadAsync()
        {
            await _client.Connect();
            await StartBroadcastHandlers(_scope);
        }


        protected override async Task OnWindowCloseAsync()
        {
            StartBeingBusy("Disconnecting client ...");
            _client.Disconnect();
            await Task.Delay(1000);
        }
    }
}
