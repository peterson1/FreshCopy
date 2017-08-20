using CommonTools.Lib.ns11.LoggingTools;
using FreshCopy.Common.API.HubServers;

namespace FreshCopy.Common.API.HubClients
{
    public interface IVersionKeeperClient : IVersionKeeperServer//, IHubClientProxy
    {
        void SetLogger(ILogList logList);
    }
}
