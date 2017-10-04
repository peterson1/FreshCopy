using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    [AuthorizeV1]
    [HubName(ClientStatusHub1.NAME)]
    public class ClientStatusHub1 : Hub
    {
        public const string NAME = "ClientStatusHub1";

        private CurrentHubClientsVM _clients;


        public ClientStatusHub1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;
        }
    }
}
