using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public static class MessageBroadcast
    {
        //public static Task ToAllClients(string message)
        //    => ToAll.BroadcastMessage(message);


        public static Task ToAllClients(string subject, string message)
            => ToAll.BroadcastMessage(subject, message);



        private static IMessageBroadcaster ToAll
            => Context.Clients.All;


        private static IHubContext<IMessageBroadcaster> Context
            => GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub1, IMessageBroadcaster>();
    }
}
