using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System;

namespace CommonTools.Lib.fx45.SignalRServers
{
    [AuthorizeV1]
    [HubName(MessageBroadcastHub1.NAME)]
    public class MessageBroadcastHub1 : Hub<IMessageBroadcaster>, IMessageBroadcastHub
    {
        public const string NAME = "MessageBroadcastHub1";

        private CurrentHubClientsVM _clients;


        public MessageBroadcastHub1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;
        }


        public override async Task OnConnected()
        {
            await Task.Delay(0);
            if (!IsValidSession(out HubClientSession session)) return;
            _clients.AddOrUpdate(session);
            await base.OnConnected();
        }


        public override async Task OnReconnected()
        {
            await Task.Delay(0);
            if (!IsValidSession(out HubClientSession session)) return;
            _clients.AddOrUpdate(session);
            await base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _clients.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        private bool IsValidSession(out HubClientSession session)
        {
            if (!Context.Request.TryGetSession(out session)) return false;
            session.ConnectionId = Context.ConnectionId;
            session.LastActivity = DateTime.Now;
            session.HubName      = MessageBroadcastHub1.NAME;
            return true;
        }


        public void ReceiveClientState(CurrentClientState clientState)
        {
            if (!IsValidSession(out HubClientSession session)) return;
            session.CurrentState = clientState;
            _clients.AddOrUpdate(session);
        }
    }
}
