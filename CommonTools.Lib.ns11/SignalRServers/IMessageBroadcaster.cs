using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.SignalRServers
{
    public interface IMessageBroadcaster
    {
        //Task BroadcastMessage(string message);
        Task BroadcastMessage(string subject, string message);
    }
}
