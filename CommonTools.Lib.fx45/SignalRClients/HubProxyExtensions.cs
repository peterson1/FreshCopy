using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRClients
{
    public static class HubProxyExtensions
    {
        public static async Task<T> InvokeUntilOK<T>(this IHubProxy hub, string method, params object[] args)
        {
            try
            {
                return await hub.Invoke<T>(method, args);
            }
            catch (InvalidOperationException)
            {
                await Task.Delay(1000);
                return await hub.InvokeUntilOK<T>(method, args);
            }
        }
    }
}
