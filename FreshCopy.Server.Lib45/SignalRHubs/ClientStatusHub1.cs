using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.SignalrTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Common.API.HubServers;
using FreshCopy.Server.Lib45.HubClientStates;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    [AuthorizeV1]
    [HubName(ClientStatusHub.Name)]
    public class ClientStatusHub1 : Hub<IClientStatusHubEvents>, IClientStatusHub
    {
        private AuthorizeHelperV1   _clients;
        private CurrentHubClientsVM _current;


        public ClientStatusHub1(AuthorizeHelperV1 authorizeHelperV1,
                                CurrentHubClientsVM currentHubClientsVM)
        {
            _current = currentHubClientsVM;
            _clients = authorizeHelperV1;
            _clients.TargetHubName = ClientStatusHub.Name;
        }


        public async Task<List<HubClientSession>> GetCurrentList()
        {
            await Task.Delay(0);
            return _current.List.ToList();
        }


        public Task RequestClientStates()
            //=> MessageBroadcast.RequestClientStates();
            => MBHub.All.RequestClientState();


        public override Task OnConnected    () => _clients.Enlist(Context);
        public override Task OnReconnected  () => _clients.Enlist(Context);
        public override Task OnDisconnected (bool stopCalled) => _clients.Delist(Context, stopCalled);
    }
}
