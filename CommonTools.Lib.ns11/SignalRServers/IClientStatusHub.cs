using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.SignalRServers
{
    public interface IClientStatusHub
    {
        Task<List<HubClientSession>> GetCurrentList();
        Task RequestClientStates ();
        Task RequestClientState  (string connectionID);
        Task RewriteConfigFile   (string encryptedDTO, string connectionID);
    }
}
