using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.SignalRClients;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.HubServers;
using System;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.HubClientProxies
{
    public partial class VersionKeeperClientProxy1 : IVersionKeeperClient
    {
 

        private async Task<string> GetText(string method, string fileKey)
        {
            string str = null;
            using (var conn = new AuthenticHubConnection1(_cfg))
            {
                conn.Error += ex => _log.Add(ex);
                try
                {
                    var hub = await conn.ConnectToHub(VersionKeeperHub.Name);
                    //str     = await hub.Invoke<string>(method, fileKey);
                    str     = await hub.InvokeUntilOK<string>(conn, method, fileKey);
                }
                catch (Exception ex) { _log.Add(ex); }
            }
            if (str.IsBlank())
                throw Fault.BadArg(nameof(fileKey), fileKey);

            return str;
        }


        private async Task<long> GetLong(string method, string fileKey)
        {
            long num = -1;
            using (var conn = new AuthenticHubConnection1(_cfg))
            {
                conn.Error += ex => _log.Add(ex);
                try
                {
                    var hub = await conn.ConnectToHub(VersionKeeperHub.Name);
                    num = await hub.Invoke<long>(method, fileKey);
                }
                catch (Exception ex) { _log.Add(ex); }
            }
            if (num < 0)
                throw Fault.BadArg(nameof(fileKey), fileKey);

            return num;
        }


        public void SetLogger(ILogList logList) => _log = logList;
   }
}
