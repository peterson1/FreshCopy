using CommonTools.Lib.fx45.SignalRClients;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRHubServers;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.HubServers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.HubClientProxies
{
    public class VersionKeeperClientProxy1 : IVersionKeeperClient
    {
        private IHubClientSettings _cfg;
        private ILogList           _log;


        public VersionKeeperClientProxy1(IHubClientSettings hubClientSettings)
        {
            _cfg = hubClientSettings;
        }


        public Task<string> GetLatestB64(string fileKey)
        {
            var methd = nameof(IVersionKeeperServer.GetLatestB64);
            return GetString(methd, fileKey);
        }


        public Task<string> GetLatestSHA1(string fileKey)
        {
            var methd = nameof(IVersionKeeperServer.GetLatestSHA1);
            return GetString(methd, fileKey);
        }


        private async Task<string> GetString(string method, string fileKey)
        {
            using (var conn = new HubConnection(_cfg.ServerURL))
            {
                conn.Error += ex => _log.Add(ex);
                string str = null;
                try
                {
                    var hub = await conn.ConnectToHub(VersionKeeperHub.Name);
                    str = await hub.Invoke<string>(method, fileKey);
                }
                catch (Exception ex)
                {
                    _log.Add(ex);
                }

                if (str.IsBlank())
                    throw Fault.BadArg(nameof(fileKey), fileKey);

                return str;
            }
        }


        public void SetLogger(ILogList logList) => _log = logList;
    }
}
