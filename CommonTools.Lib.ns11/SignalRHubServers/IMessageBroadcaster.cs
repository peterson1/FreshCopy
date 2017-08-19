using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.SignalRHubServers
{
    public interface IMessageBroadcaster
    {
        Task BroadcastMessage(string message);
    }
}
