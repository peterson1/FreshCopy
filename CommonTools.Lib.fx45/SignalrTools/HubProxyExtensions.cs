using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalrTools
{
    public static class HubProxyExtensions
    {
        public static async Task<T> InvokeUntilOK<T>(this IHubProxy hub, HubConnection conn, string method, params object[] args)
        {
            try
            {
                return await hub.Invoke<T>(method, args);
            }
            catch (InvalidOperationException)
            {
                while (conn.State != ConnectionState.Connected)
                    await Task.Delay(1000);

                return await hub.InvokeUntilOK<T>(conn, method, args);
            }
        }
    }
}
