using CommonTools.Lib.ns11.SignalRClients;

namespace CommonTools.Lib.ns11.SignalRServers
{
    public interface IClientStatusHubEvents
    {
        void ClientConnected    (HubClientSession sessionInfo);
        void ClientInteracted   (HubClientSession sessionInfo);
        void ClientDisconnected (HubClientSession sessionInfo);
    }
}
