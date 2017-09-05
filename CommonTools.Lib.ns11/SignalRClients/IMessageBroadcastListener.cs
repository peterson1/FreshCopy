using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.SignalRClients
{
    public interface IMessageBroadcastListener : IHubClientProxy
    {
        event EventHandler<KeyValuePair<string, string>> BroadcastReceived;
        Task SendClientState(CurrentClientState state);
    }
}
