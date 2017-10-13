using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.ns11.FileSystemTools;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Server.Lib45.SignalRHubs;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.FileWatchers
{
    public class BinaryFileWatcherVM : FileWatcherVMBase
    {
        public BinaryFileWatcherVM(IThrottledFileWatcher throttledFileWatcher, 
                                   SharedLogListVM commonLogListVM) 
            : base(throttledFileWatcher, commonLogListVM)
        {
        }


        protected override void OnFileChanged(string fileKey, string filePath)
        {
            var subj = typeof(BinaryFileChangeInfo).Name;
            var msg  = ComposeBroadcastMessage(fileKey, filePath);
            Task.Run(async () =>
            {
                //await MessageBroadcast.ToAllClients(subj, msg);
                await MBHub.BroadcastToAll(subj, msg);
            });
        }


        private string ComposeBroadcastMessage(string fileKey, string filePath)
        {
            var desc = new BinaryFileChangeInfo
            {
                FileKey = fileKey,
                NewSHA1 = filePath.SHA1ForFile()
            };
            return JsonConvert.SerializeObject(desc);
        }
    }
}
