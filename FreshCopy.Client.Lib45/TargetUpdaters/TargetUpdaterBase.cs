using CommonTools.Lib.ns11.LoggingTools;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.TargetUpdaters;
using System;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.TargetUpdaters
{
    public abstract class TargetUpdaterBase<T> : ITargetUpdater<T>
        where T : ITargetChangeInfo
    {
        protected IVersionKeeperClient _client;
        protected string               _fileKey;
        protected string               _filePath;
        private   ILogList             _logs;
        private   bool                 _isBusy;


        public TargetUpdaterBase(IVersionKeeperClient versionKeeperClient)
        {
            _client = versionKeeperClient;
        }


        protected abstract Task ApplyChangesIfNeededAsync(T change);


        public void ApplyChangesIfNeeded(T change)
        {
            if (_isBusy)
            {
                Log($"‹{GetType().Name}› cannot process the request to [{nameof(ITargetUpdater<T>.ApplyChangesIfNeeded)}] while a previous request is running.");
                return;
            }
            _isBusy = true;

            Task.Run(async () =>
            {
                try
                {
                    await ApplyChangesIfNeededAsync(change);
                }
                catch (Exception ex)
                {
                    _logs.Add(ex);
                }
                _isBusy = false;
            });
        }


        public void SetTarget(string fileKey, string filePath, ILogList logList)
        {
            _fileKey  = fileKey;
            _filePath = filePath;
            _logs     = logList;
            _client.SetLogger(logList);
        }


        protected void Log(string message) => _logs.Add(message);
    }
}
