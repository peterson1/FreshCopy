using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRClients
{
    public class MessageBroadcastListener1 : IMessageBroadcastListener
    {
        private const string HUBNAME = MessageBroadcastHub1.NAME;
        private const string METHOD  = MessageBroadcastHub1.METHOD;

        private      EventHandler<string> _broadcastReceived;
        public event EventHandler<string>  BroadcastReceived
        {
            add    { _broadcastReceived -= value; _broadcastReceived += value; }
            remove { _broadcastReceived -= value; }
        }


        private IHubClientSettings _cfg;
        private HubConnection      _conn;
        private IHubProxy          _hub;


        public MessageBroadcastListener1(IHubClientSettings hubClientSettings)
        {
            _cfg = hubClientSettings;
        }


        public async Task Connect()
        {
            _conn     = new HubConnection   (_cfg.ServerURL);
            _hub      = _conn.CreateHubProxy(HUBNAME);

            _hub.On<string>(METHOD, msg 
                => _broadcastReceived?.Invoke(this, msg));

            await _conn.Start();
        }


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
