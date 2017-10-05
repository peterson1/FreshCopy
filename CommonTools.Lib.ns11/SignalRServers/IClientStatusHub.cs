using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;

namespace CommonTools.Lib.ns11.SignalRServers
{
    public interface IClientStatusHub
    {
        List<HubClientSession> GetCurrentList();
    }
}
