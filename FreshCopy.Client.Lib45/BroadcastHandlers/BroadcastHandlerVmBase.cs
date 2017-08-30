using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Common.API.TargetUpdaters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public abstract class BroadcastHandlerVmBase<T> : ViewModelBase, IBroadcastHandler
        where T : ITargetChangeInfo
    {
        private ITargetUpdater<T>         _updatr;
        private IMessageBroadcastListener _listnr;


        public BroadcastHandlerVmBase(IMessageBroadcastListener messageBroadcastListener,
                                      ContextLogListVM contextLogListVM,
                                      ITargetUpdater<T> targetUpdater)
        {
            _updatr = targetUpdater;
            _listnr = messageBroadcastListener;
            Logs    = contextLogListVM;
        }


        public string             FileKey  { get; private set; }
        public ContextLogListVM   Logs     { get; }


        private void OnBroadcastReceived(object sender, KeyValuePair<string, string> kvp)
        {
            if (IsRelevantMessage(kvp, out T chnge))
                _updatr.OnChangeReceived(chnge);
        }


        private bool IsRelevantMessage(KeyValuePair<string, string> kvp, out T chnge)
        {
            chnge = default(T);
            if (kvp.Key != typeof(T).Name) return false;
            chnge = JsonConvert.DeserializeObject<T>(kvp.Value);
            if (chnge == null) return false;
            return chnge.FileKey == FileKey;
        }


        public void SetTargetFile(string fileKey, string filePath)
        {
            FileKey  = fileKey;
            _updatr.SetTarget(fileKey, filePath, Logs);
        }


        public async Task CheckThenSetHandler()
        {
            await _updatr.RunInitialCheck();
            _listnr.BroadcastReceived += OnBroadcastReceived;
        }
    }
}
