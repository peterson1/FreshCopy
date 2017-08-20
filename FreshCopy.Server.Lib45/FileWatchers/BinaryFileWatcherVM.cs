using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using System.IO;
using System.Threading.Tasks;
using System;
using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.ChangeDescriptions;
using Newtonsoft.Json;

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
                await MessageBroadcast.ToAllClients(subj, msg);
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
