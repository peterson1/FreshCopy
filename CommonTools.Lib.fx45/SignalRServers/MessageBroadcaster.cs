using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public static class MessageBroadcast
    {
        public static async Task ToAllClients(string message)
        {
            var ctx = GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub1, IMessageBroadcaster>();
            await ctx.Clients.All.BroadcastMessage(message);
        }
    }
}
