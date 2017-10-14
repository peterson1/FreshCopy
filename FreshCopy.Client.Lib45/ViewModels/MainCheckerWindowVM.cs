﻿using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.BroadcastHandlers;
using FreshCopy.Client.Lib45.ScreenshotTools;
using FreshCopy.Common.API.Configuration;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class MainCheckerWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Update Checker";

        private IMessageBroadcastClient      _client;
        private StateRequestBroadcastHandler _reqHandlr;
        private CfgEditorHubEventHandler     _cfgEditHandlr;

        public MainCheckerWindowVM(UpdateCheckerSettings updateCheckerSettings,
                                   IMessageBroadcastClient messageBroadcastListener,
                                   SharedLogListVM commonLogListVM,
                                   StateRequestBroadcastHandler stateRequestBroadcastHandler,
                                   CfgEditorHubEventHandler cfgEditorHubEventHandler,
                                   ScreenshotSenderVM screenshotSenderVM)
        {
            _client        = messageBroadcastListener;
            _reqHandlr     = stateRequestBroadcastHandler;
            _cfgEditHandlr = cfgEditorHubEventHandler;
            Config         = updateCheckerSettings;
            CommonLogs     = commonLogListVM;
            Screenshooter  = screenshotSenderVM;

            _client.StateChanged += (s, e)
                => AppendToCaption(e);

            _client.BroadcastReceived += (s, e) 
                => CommonLogs.Add($"[{e.Key}]  {e.Value}");

            if (Config.CanExitApp != true)
                ExitCmd.OverrideEnabled = false;
        }


        public ScreenshotSenderVM     Screenshooter { get; }
        public UpdateCheckerSettings  Config        { get; }
        public SharedLogListVM        CommonLogs    { get; }


        public ObservableCollection<IChangeBroadcastHandler> Listeners { get; } = new ObservableCollection<IChangeBroadcastHandler>();


        public async Task StartBroadcastHandlers()
        {
            if (Config.UpdateSelf == true)
                await StartNewHandler<BinaryFileChangeBroadcastHandlerVM>(
                    CheckerRelease.FileKey, CurrentExe.GetFullPath());

            foreach (var kv in Config.BinaryFiles)
                await StartNewHandler<BinaryFileChangeBroadcastHandlerVM>(kv.Key, kv.Value);

            foreach (var kv in Config.AppendOnlyDBs)
                await StartNewHandler<AppendOnlyDbChangeBroadcastHandlerVM>(kv.Key, kv.Value);
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
            await _client.Connect();
            await StartBroadcastHandlers();
        }


        protected override async Task OnWindowCloseAsync()
        {
            StartBeingBusy("Disconnecting client ...");
            _client.Disconnect();
            await Task.Delay(1000);
        }
    }
}
