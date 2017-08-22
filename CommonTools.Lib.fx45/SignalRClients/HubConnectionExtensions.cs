using Microsoft.AspNet.SignalR.Client;
using System;
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


        public static async Task TryStartUntilConnected(this HubConnection conn, Action<Exception> errorHandlr)
        {
            while (conn.State != ConnectionState.Connected)
            {
                try
                {
                    await conn.Start();
                }
                catch (Exception ex)
                {
                    errorHandlr(ex);
                    await Task.Delay(1000 * 5);
                }
            }
        }
    }
}
