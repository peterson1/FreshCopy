using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public static class MessageBroadcast
    {
        public static Task RequestClientStates()
            => ToAllClients("Request to Client", typeof(CurrentClientState).Name);


        public static async Task ToAllClients(string subject, string message)
        {
            try
            {
                await ToAll.BroadcastMessage(subject, message);
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "ToAll.BroadcastMessage");
            }
        }


        private static IMessageBroadcaster ToAll
            => Context.Clients.All;


        private static IHubContext<IMessageBroadcaster> Context
            => GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub1, IMessageBroadcaster>();
    }
}
