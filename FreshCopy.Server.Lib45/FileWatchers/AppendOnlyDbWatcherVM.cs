using CommonTools.Lib.fx45.LiteDbTools;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using FreshCopy.Common.API.ChangeDescriptions;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.FileWatchers
{
    public class AppendOnlyDbWatcherVM : FileWatcherVMBase
    {
        public AppendOnlyDbWatcherVM(IThrottledFileWatcher throttledFileWatcher, 
                                     SharedLogListVM commonLogListVM) 
            : base(throttledFileWatcher, commonLogListVM)
        {
        }


        protected override void OnFileChanged(string fileKey, string filePath)
        {
            var subj = typeof(AppendOnlyDbChangeInfo).Name;
            var msg  = ComposeBroadcastMessage(fileKey, filePath);
            Task.Run(async () =>
            {
                await MessageBroadcast.ToAllClients(subj, msg);
            });
        }


        private string ComposeBroadcastMessage(string fileKey, string filePath)
        {
            var desc = new AppendOnlyDbChangeInfo
            {
                FileKey = fileKey,
                MaxId   = AnyLiteDB.GetMaxId(filePath)
            };
            return JsonConvert.SerializeObject(desc);
        }
    }
}
