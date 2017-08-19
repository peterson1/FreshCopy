using CommonTools.Lib.fx45.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    //[HubName(MessageBroadcastHub1.NAME)]
    public class VersionKeeperHub1 : Hub
    {
        private CurrentHubClientsVM _clients;


        public VersionKeeperHub1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;
        }


        public override Task OnConnected()
        {
            _clients.Add(Context.ConnectionId);
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _clients.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
