using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Common.API.HubServers;
using FreshCopy.Server.Lib45.HubClientStates;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    [AuthorizeV1]
    [HubName(MessageBroadcastHub.Name)]
    public class MessageBroadcastHub1 : Hub<IMessageBroadcastHubEvents>, IMessageBroadcastHub
    {
        private AuthorizeHelperV1 _clients;


        public MessageBroadcastHub1(AuthorizeHelperV1 authorizeHelperV1)
        {
            _clients = authorizeHelperV1;
            _clients.TargetHubName = MessageBroadcastHub.Name;
        }


        public void ReceiveClientState(CurrentClientState clientState)
        {
            _clients.Enlist(Context, clientState);
        }


        public void ReceiveException(ExceptionReport exceptionReport)
        {
            _clients.AddError(Context, exceptionReport);
        }


        public override Task OnConnected    () => _clients.Enlist(Context);
        public override Task OnReconnected  () => _clients.Enlist(Context);
        public override Task OnDisconnected (bool stopCalled) => _clients.Delist(Context, stopCalled);


        public static IHubContext<IMessageBroadcastHubEvents> HubContext
            => GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub1, IMessageBroadcastHubEvents>();
    }
}
