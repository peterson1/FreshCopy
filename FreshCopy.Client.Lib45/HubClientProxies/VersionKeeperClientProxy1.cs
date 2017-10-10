using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.SignalrTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.HubServers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.HubClientProxies
{
    public partial class VersionKeeperClientProxy1 : IVersionKeeperClient
    {
        private IMessageBroadcastClient _client2;
        private IHubClientSettings      _cfg;
        private ILogList                _log;


        public VersionKeeperClientProxy1(IHubClientSettings hubClientSettings,
                                         IMessageBroadcastClient messageBroadcastClient)
        {
            _client2 = messageBroadcastClient;
            _cfg     = hubClientSettings;
        }


        public async Task<List<string>> GetRecords(string fileKey, long startId)
        {
            List<string> list = null;
            var method = nameof(IVersionKeeperServer.GetRecords);

            using (var conn = new AuthenticHubConnection1(_cfg))
            {
                conn.Error += ex => _log.Add(ex);
                try
                {
                    var hub = await conn.ConnectToHub(VersionKeeperHub.Name);
                    list    = await hub.Invoke<List<string>>(method, fileKey, startId);
                }
                catch (Exception ex) { _log.Add(ex); }
            }
            if (list == null)
                throw Fault.BadArg(nameof(fileKey), fileKey);

            return list;
        }


        public Task<string> GetContentB64(string fileKey)
        {
            var methd = nameof(IVersionKeeperServer.GetContentB64);
            return GetText(methd, fileKey);
        }


        public Task<string> GetLatestSHA1(string fileKey)
        {
            var methd = nameof(IVersionKeeperServer.GetLatestSHA1);
            return GetText(methd, fileKey);
        }


        public Task<long> GetMaxId(string fileKey)
        {
            var methd = nameof(IVersionKeeperServer.GetMaxId);
            return GetLong(methd, fileKey);
        }
    }
}
