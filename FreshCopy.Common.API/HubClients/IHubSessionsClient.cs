using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using System;

namespace FreshCopy.Common.API.HubClients
{
    public interface IHubSessionsClient : IClientStatusHub, IHubClientProxy
    {
        event EventHandler<HubClientSession> ClientConnected;
        event EventHandler<HubClientSession> ClientInteracted;
        event EventHandler<HubClientSession> ClientDisconnected;
    }
}
