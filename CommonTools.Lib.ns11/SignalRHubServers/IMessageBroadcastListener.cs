using System;

namespace CommonTools.Lib.ns11.SignalRHubServers
{
    public interface IMessageBroadcastListener : IHubClientProxy
    {
        event EventHandler<string> BroadcastReceived;
    }
}
