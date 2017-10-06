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
    public class MessageBroadcastHub1 : Hub<IMessageBroadcastHubEvents>, IMessageBroadcastHub
    {
        public const string NAME = "MessageBroadcastHub1";

        private AuthorizeHelperV1   _clients;


        public MessageBroadcastHub1(AuthorizeHelperV1 authorizeHelperV1)
        {
            _clients = authorizeHelperV1;
            _clients.TargetHubName = MessageBroadcastHub1.NAME;
        }


        public void ReceiveClientState(CurrentClientState clientState)
        {
            _clients.Enlist(Context, clientState);
        }


        public override Task OnConnected    () => _clients.Enlist(Context);
        public override Task OnReconnected  () => _clients.Enlist(Context);
        public override Task OnDisconnected (bool stopCalled) => _clients.Delist(Context, stopCalled);
    }
}
