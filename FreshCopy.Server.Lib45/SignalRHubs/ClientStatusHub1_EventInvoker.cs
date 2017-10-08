using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;
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
