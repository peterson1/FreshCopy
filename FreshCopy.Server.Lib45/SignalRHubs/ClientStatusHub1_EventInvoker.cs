using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    public static class ClientStateListeners
    {
        public static IClientStatusHubEvents Notify => Clients.All;


        private static IHubConnectionContext<IClientStatusHubEvents> Clients
            => GlobalHost.ConnectionManager.GetHubContext<ClientStatusHub1, IClientStatusHubEvents>().Clients;
    }
}
