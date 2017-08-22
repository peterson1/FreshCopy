using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.TargetUpdaters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public abstract class BroadcastHandlerVmBase<T> : ViewModelBase, IBroadcastHandler
        where T : ITargetChangeInfo
    {
        private ITargetUpdater<T> _updatr;


        public BroadcastHandlerVmBase(IMessageBroadcastListener listenr,
                                      ContextLogListVM contextLogListVM,
                                      ITargetUpdater<T> targetUpdater)
        {
            _updatr = targetUpdater;
            Logs    = contextLogListVM;
            listenr.BroadcastReceived += OnBroadcastReceived;
        }


        public string             FileKey  { get; private set; }
        public ContextLogListVM   Logs     { get; }


        //protected abstract void OnChangeReceived(T changeInfo);


        private void OnBroadcastReceived(object sender, KeyValuePair<string, string> kvp)
        {
            if (IsRelevantMessage(kvp, out T chnge))
                _updatr.ApplyChangesIfNeeded(chnge);
                //OnChangeReceived(chnge);
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
            //OnTargetAcquired(fileKey, filePath);
            _updatr.SetTarget(fileKey, filePath, Logs);
        }


        //protected virtual void OnTargetAcquired(string fileKey, string filePath)
        //{
        //}
    }
}
