using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using System.Threading.Tasks;
using System;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    public static class MBHub
    {
        public static Task RequestSessionUpdate(string connectionId)
            => Client(connectionId).RequestClientState();


        public static IMessageBroadcastHubEvents Client(string connectionId)
            => MessageBroadcastHub1.HubContext
                .Clients.Client(connectionId);


        public static IMessageBroadcastHubEvents All
            => MessageBroadcastHub1.HubContext.Clients.All;


        public static Task RequestClientState(this IMessageBroadcastHubEvents hubEvts)
            => hubEvts.BroadcastMessage("Request to Client", typeof(CurrentClientState).Name);


        public static Task BroadcastToAll(string subj, string msg)
            => All.BroadcastMessage(subj, msg);
    }
}
