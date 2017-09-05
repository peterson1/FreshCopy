using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public static class MessageBroadcast
    {
        public static Task RequestClientStates()
            => ToAllClients("Request to Client", typeof(CurrentClientState).Name);


        public static Task ToAllClients(string subject, string message)
            => ToAll.BroadcastMessage(subject, message);


        private static IMessageBroadcaster ToAll
            => Context.Clients.All;


        private static IHubContext<IMessageBroadcaster> Context
            => GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub1, IMessageBroadcaster>();
    }
}
