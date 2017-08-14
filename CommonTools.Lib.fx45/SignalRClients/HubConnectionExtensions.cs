using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRClients
{
    public static class HubConnectionExtensions
    {
        public static async Task<IHubProxy> ConnectToHub(this HubConnection conn, string hubName)
        {
            var hub = conn.CreateHubProxy(hubName);
            await conn.Start();
            return hub;
        }
    }
}
