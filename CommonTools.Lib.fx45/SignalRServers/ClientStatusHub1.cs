using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    [AuthorizeV1]
    [HubName(ClientStatusHub1.NAME)]
    public class ClientStatusHub1 : Hub<IClientStatusHubEvents>, IClientStatusHub
    {
        public const string NAME = "ClientStatusHub1";

        private AuthorizeHelperV1   _clients;
        private CurrentHubClientsVM _current;


        public ClientStatusHub1(AuthorizeHelperV1 authorizeHelperV1,
                                CurrentHubClientsVM currentHubClientsVM)
        {
            _current = currentHubClientsVM;
            _clients = authorizeHelperV1;
            _clients.TargetHubName = ClientStatusHub1.NAME;
        }


        public List<HubClientSession> GetCurrentList()
            => _current.List.ToList();


        public override Task OnConnected    () => _clients.Enlist(Context);
        public override Task OnReconnected  () => _clients.Enlist(Context);
        public override Task OnDisconnected (bool stopCalled) => _clients.Delist(Context, stopCalled);
    }
}
