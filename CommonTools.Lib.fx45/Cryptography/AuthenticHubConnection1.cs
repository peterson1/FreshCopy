using CommonTools.Lib.ns11.SignalRClients;
using Microsoft.AspNet.SignalR.Client;

namespace CommonTools.Lib.fx45.Cryptography
{
    public class AuthenticHubConnection1 : HubConnection
    {

        public AuthenticHubConnection1(IHubClientSettings cfg) : base(cfg.ServerURL)
        {
            this.Headers.AddHmacCyphers(cfg);
        }
    }
}
