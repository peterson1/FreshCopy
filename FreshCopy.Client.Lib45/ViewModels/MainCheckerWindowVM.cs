using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Client.Lib45.BroadcastHandlers;
using FreshCopy.Common.API.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class MainCheckerWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Update Checker";

        private IMessageBroadcastClient      _client;
        private StateRequestBroadcastHandler _reqHandlr;
        private CfgEditorHubEventHandler     _cfgEditHandlr;
        private TrayContextMenuItems         _trayMenu;
        private JobOrderWatcher              _jobWatchr;

        public MainCheckerWindowVM(UpdateCheckerSettings updateCheckerSettings,
                                   IMessageBroadcastClient messageBroadcastListener,
                                   SharedLogListVM commonLogListVM,
                                   StateRequestBroadcastHandler stateRequestBroadcastHandler,
                                   CfgEditorHubEventHandler cfgEditorHubEventHandler,
                                   TrayContextMenuItems trayContextMenuItems)
        {
            _client         = messageBroadcastListener;
            _reqHandlr      = stateRequestBroadcastHandler;
            _cfgEditHandlr  = cfgEditorHubEventHandler;
            _trayMenu       = trayContextMenuItems;
            Config          = updateCheckerSettings;
            CommonLogs      = commonLogListVM;

            _client.StateChanged += (s, e)
                => AppendToCaption(e);

            _client.BroadcastReceived += (s, e) 
                => CommonLogs.Add($"[{e.Key}]  {e.Value}");

            if (Config.CanExitApp != true)
                ExitCmd.OverrideEnabled = false;
        }


        public UpdateCheckerSettings  Config          { get; }
        public SharedLogListVM        CommonLogs      { get; }


        public ObservableCollection<IChangeBroadcastHandler> Listeners { get; } = new ObservableCollection<IChangeBroadcastHandler>();


        public async Task StartBroadcastHandlers()
        {
            await SetFirebaseHandler();

            if (Config.UpdateSelf == true)
                await StartNewHandler<BinaryFileChangeBroadcastHandlerVM>(
                    CheckerRelease.FileKey, CurrentExe.GetFullPath());

            foreach (var kv in Config.BinaryFiles)
                await StartNewHandler<BinaryFileChangeBroadcastHandlerVM>(kv.Key, kv.Value);

            foreach (var kv in Config.AppendOnlyDBs)
                await StartNewHandler<AppendOnlyDbChangeBroadcastHandlerVM>(kv.Key, kv.Value);

            foreach (var kv in Config.Executables)
                await StartNewHandler<BinaryFileChangeBroadcastHandlerVM>(kv.Key, kv.Value);
        }


        private async Task SetFirebaseHandler()
        {
            if (Config.FirebaseURL.IsBlank()) return;

            _jobWatchr = new JobOrderWatcher(Config.FirebaseURL,
                            Config.FirebaseKey, Config.FirebaseUsr, 
                            Config.FirebasePwd, Config.UserAgent);


            await _jobWatchr.StartWatching(async cmd =>
            {
                await Loggly.Post(
                    $"Unstarted job found: “{cmd.Command}”");

                return JobResult.Success("Message posted to Logggly.");
            });
        }


        private async Task StartNewHandler<T>(string fileKey, string filePath)
            where T : class, IChangeBroadcastHandler
        {
            var listnr = Resolve<T>();
            listnr.SetTargetFile(fileKey, filePath);
            UIThread.Run(() => Listeners.Add(listnr));
            await listnr.CheckThenSetHandler();
        }


        protected override async Task OnWindowLoadAsync()
        {
            await Loggly.Post("Loading main checker window ...");
            await _client.Connect();
            await StartBroadcastHandlers();
        }


        protected override void OnWindowClose()
        {
            _jobWatchr?.Dispose();
        }


        protected override async Task OnWindowCloseAsync()
        {
            StartBeingBusy("Disconnecting client ...");
            _client.Disconnect();
            await Task.Delay(1000);
        }


        public void SetContextMenu(ContextMenu ctxMenu) 
            => _trayMenu.SetItems(ctxMenu, this);


        protected override async void OnError(Exception ex, string taskDescription = null)
        {
            await Loggly.Post(ex, taskDescription);
        }
    }
}
