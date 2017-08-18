using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    [HubName(MessageBroadcastHub1.NAME)]
    public class MessageBroadcastHub1 : Hub
    {
        public const string NAME   = "MessageBroadcastHub1";
        public const string METHOD = "BroadcastMessage";

        private CurrentHubClientsVM _clients;


        public MessageBroadcastHub1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;
        }
    }
}
