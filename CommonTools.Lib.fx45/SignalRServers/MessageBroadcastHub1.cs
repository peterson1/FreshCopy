using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    [HubName(MessageBroadcastHub1.NAME)]
    public class MessageBroadcastHub1 : Hub<IMessageBroadcaster>
    {
        public const string NAME = "MessageBroadcastHub1";

        private CurrentHubClientsVM _clients;


        public MessageBroadcastHub1(CurrentHubClientsVM activeHubClientsList)
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
