using CommonTools.Lib.fx45.LiteDbTools;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.TargetUpdaters;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.TargetUpdaters
{
    public class AppendOnlyDbUpdater1 : TargetUpdaterBase<AppendOnlyDbChangeInfo>, IAppendOnlyDbUpdater
    {
        public AppendOnlyDbUpdater1(IVersionKeeperClient versionKeeperClient) : base(versionKeeperClient)
        {
        }


        private async Task InsertNewRecordsIfOutdated(long remoteMaxId)
        {
            if (SameMaxIDs(remoteMaxId, out long localMaxId)) return;

            await QueryServerAndInsertToLocal(localMaxId + 1);

            var newerRemoteId = await _client.GetMaxId(_fileKey);
            await InsertNewRecordsIfOutdated(newerRemoteId);
        }


        private bool SameMaxIDs(long remoteMaxId, out long localMaxId)
        {
            if (!File.Exists(_filePath))
            {
                Log("Local DB currently does not exist.");
                localMaxId = 0;
                return false;
            }
            localMaxId = AnyLiteDB.GetMaxId(_filePath);
            var isSame = localMaxId == remoteMaxId;
            Log(isSame ? $"Local and remote max IDs are both [{localMaxId}]."
                       : $"Local max ID [{localMaxId}] differs from that of remote [{remoteMaxId}].");
            return isSame;
        }


        private async Task QueryServerAndInsertToLocal(long startId)
        {
            Log("Querying latest records from server ...");
            var recs = await _client.GetRecords(_fileKey, startId);
            if (recs == null)
                Log($"Something went wrong at {nameof(QueryServerAndInsertToLocal)}!");

            Log("Inserting queried records to local DB ...");
            InsertRecordsToLocalDB(recs);
        }


        private void InsertRecordsToLocalDB(List<string> recs)
        {
            AnyLiteDB.Insert(_filePath, recs);
        }


        protected override Task ApplyChangesIfNeededAsync(AppendOnlyDbChangeInfo change)
            => InsertNewRecordsIfOutdated(change.MaxId);
    }
}
