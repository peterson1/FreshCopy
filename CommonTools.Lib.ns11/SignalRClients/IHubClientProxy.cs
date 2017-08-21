using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.SignalRClients
{
    public interface IHubClientProxy : IDisposable
    {
        Task  Connect    ();
        void  Disconnect ();
    }
}
