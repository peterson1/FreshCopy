using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.SignalRServers
{
    public interface IMessageBroadcastHubEvents
    {
        Task BroadcastMessage(string subject, string message);
        void RewriteConfigFile(string encryptedDTO);
    }
}
