using System;
using System.Collections.Generic;

namespace CommonTools.Lib.ns11.SignalRClients
{
    public interface IMessageBroadcastListener : IHubClientProxy
    {
        event EventHandler<KeyValuePair<string, string>> BroadcastReceived;
    }
}
