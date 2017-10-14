using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.SignalrTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.EventHandlerTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Common.API.HubServers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.HubClientProxies
{
    public class MessageBroadcastHubProxy1 : IMessageBroadcastClient
    {
        private const string HUBNAME = MessageBroadcastHub.Name;

        private      EventHandler<KeyValuePair<string, string>> _broadcastReceived;
        public event EventHandler<KeyValuePair<string, string>>  BroadcastReceived
        {
            add    { _broadcastReceived -= value; _broadcastReceived += value; }
            remove { _broadcastReceived -= value; }
        }

        private      EventHandler<string> _configRewriteRequested;
        public event EventHandler<string>  ConfigRewriteRequested
        {
            add    { _configRewriteRequested -= value; _configRewriteRequested += value; }
            remove { _configRewriteRequested -= value; }
        }

        private      EventHandler<string> _stateChanged;
        public event EventHandler<string>  StateChanged
        {
            add    { _stateChanged -= value; _stateChanged += value; }
            remove { _stateChanged -= value; }
        }


        private IHubClientSettings _cfg;
        private HubConnection      _conn;
        private IHubProxy          _hub;
        private SharedLogListVM    _log;


        public MessageBroadcastHubProxy1(IHubClientSettings hubClientSettings, SharedLogListVM sharedLogListVM)
        {
            _cfg = hubClientSettings;
            _log = sharedLogListVM;
        }


        public async Task Connect()
        {
            if (_conn != null) return;
            _conn = new AuthenticHubConnection1(_cfg);
            _hub = _conn.CreateHubProxy(HUBNAME);

            _conn.StateChanged += e
                => _stateChanged?.Raise(e.NewState.ToString());

            SetHubEventHandlers();

            await _conn.TryStartUntilConnected(ex => _log.Add(ex.Message));
        }


        private void SetHubEventHandlers()
        {
            var method = nameof(IMessageBroadcastHubEvents.BroadcastMessage);
            _hub.On<string, string>(method, (subj, msg)
                => _broadcastReceived?.Invoke(this, new KeyValuePair<string, string>(subj, msg)));

            method = nameof(IMessageBroadcastHubEvents.RewriteConfigFile);
            _hub.On<string>(method, encryptedDTO
                => _configRewriteRequested?.Invoke(this, encryptedDTO));
        }


        public void SendClientState(CurrentClientState state)
        {
            if (!IsValidConnState()) return;
            var method = nameof(IMessageBroadcastHub.ReceiveClientState);
            _hub.Invoke(method, state);
        }


        private bool IsValidConnState([CallerMemberName] string callingMethod = null)
        {
            if (_conn == null || _hub == null)
                throw Fault.CallFirst(nameof(Connect), callingMethod);

            if (_conn.State != ConnectionState.Connected)
            {
                _log.Add($"Method [{callingMethod}] failed because current connection is [{_conn.State}].");
                return false;
            }
            return true;
        }


        public void SendException(string context, Exception ex)
        {
            if (!IsValidConnState()) return;
            var method = nameof(IMessageBroadcastHub.ReceiveException);
            var report = new ExceptionReport(context, ex);
            _hub.Invoke(method, report);
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
