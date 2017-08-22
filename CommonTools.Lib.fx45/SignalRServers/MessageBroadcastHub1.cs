using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    [AuthorizeV1]
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
            Context.Request.TryGetSession(out HubClientSession session);
            _clients.Add(Context.ConnectionId, session);
            return base.OnConnected();
        }


        public override Task OnReconnected()
        {
            Context.Request.TryGetSession(out HubClientSession session);
            _clients.Add(Context.ConnectionId, session);
            return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _clients.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}
