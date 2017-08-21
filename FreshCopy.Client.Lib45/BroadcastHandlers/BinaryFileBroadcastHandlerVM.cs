using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.TargetUpdaters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class BinaryFileBroadcastHandlerVM : ViewModelBase
    {
        private IBinaryFileUpdater _updtr;


        public BinaryFileBroadcastHandlerVM(IMessageBroadcastListener listenr,
                                            IBinaryFileUpdater binaryFileUpdater,
                                            ContextLogListVM contextLogListVM)
        {
            _updtr  = binaryFileUpdater;
            Logs    = contextLogListVM;
            listenr.BroadcastReceived += OnBroadcastReceived;
        }


        public string             FileKey  { get; private set; }
        public ContextLogListVM   Logs     { get; }


        private void OnBroadcastReceived(object sender, KeyValuePair<string, string> kvp)
        {
            if (IsRelevantMessage(kvp, out BinaryFileChangeInfo chnge))
                _updtr.ApplyChangesIfNeeded(chnge.NewSHA1);
        }


        private bool IsRelevantMessage(KeyValuePair<string, string> kvp, out BinaryFileChangeInfo chnge)
        {
            chnge = null;
            if (kvp.Key != typeof(BinaryFileChangeInfo).Name) return false;
            chnge = JsonConvert.DeserializeObject<BinaryFileChangeInfo>(kvp.Value);
            if (chnge == null) return false;
            return chnge.FileKey == FileKey;
        }


        public void SetTargetFile(string fileKey, string filePath)
        {
            FileKey  = fileKey;
            _updtr.SetTarget(fileKey, filePath, Logs);
        }
    }
}
