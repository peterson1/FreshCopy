using FreshCopy.Common.API.ServiceContracts;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    [HubName("VersionKeeperHub")]
    public class VersionKeeperHub1 : Hub, IFileUpdater
    {
    }
}
