using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FreshCopy.Client.Lib45.HubClientProxies
{
    public class VersionKeeperClientProxy1 : IHubClientProxy
    {
        private IHubClientSettings     _cfg;
        private HubConnection          _conn;
        private IHubProxy              _hub;
        private SynchronizationContext _ui;


        public VersionKeeperClientProxy1(IHubClientSettings hubClientSettings)
        {
            _cfg = hubClientSettings;
            _ui  = SynchronizationContext.Current;
        }


        public async Task Connect()
        {
            _conn = new HubConnection(_cfg.ServerURL);
            //_conn.TraceLevel = TraceLevels.All;
            //_conn.TraceWriter = Console.Out;

            _hub  = _conn.CreateHubProxy(_cfg.HubName);
            _hub.On<string>("SendMessage", msg =>
            {
                AsUI(_ => MessageBox.Show(msg));
            });

            await _conn.Start();

            //await _hub.Invoke("SendMessage", "from client");
        }


        //public void SendMessage(string msg)
        //{
        //    AsUI(_ => MessageBox.Show(msg));
        //}



        public void Disconnect()
        {
            try
            {
                _hub = null;
                _conn?.Dispose();
                _conn = null;
            }
            catch { }
        }



        public void AsUI(SendOrPostCallback action)
            => _ui.Send(action, null);


        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disconnect();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
