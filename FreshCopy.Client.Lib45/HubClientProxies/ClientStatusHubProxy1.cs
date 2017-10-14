using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.SignalrTools;
using CommonTools.Lib.ns11.EventHandlerTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.HubServers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.HubClientProxies
{
    public class ClientStatusHubProxy1 : IHubSessionsClient
    {
        private const string HUBNAME = ClientStatusHub.Name;

        private      EventHandler<string> _stateChanged;
        public event EventHandler<string>  StateChanged
        {
            add    { _stateChanged -= value; _stateChanged += value; }
            remove { _stateChanged -= value; }
        }

        private      EventHandler<HubClientSession> _clientConnected;
        public event EventHandler<HubClientSession>  ClientConnected
        {
            add    { _clientConnected -= value; _clientConnected += value; }
            remove { _clientConnected -= value; }
        }

        private      EventHandler<HubClientSession> _clientInteracted;
        public event EventHandler<HubClientSession>  ClientInteracted
        {
            add    { _clientInteracted -= value; _clientInteracted += value; }
            remove { _clientInteracted -= value; }
        }

        private      EventHandler<HubClientSession> _clientDisconnected;
        public event EventHandler<HubClientSession>  ClientDisconnected
        {
            add    { _clientDisconnected -= value; _clientDisconnected += value; }
            remove { _clientDisconnected -= value; }
        }

        private IHubClientSettings   _cfg;
        private SharedLogListVM      _log;
        private HubConnection        _conn;
        private IHubProxy            _hub;


        public ClientStatusHubProxy1(IHubClientSettings hubClientSettings,
                                     SharedLogListVM sharedLogListVM)
        {
            _cfg = hubClientSettings;
            _log = sharedLogListVM;
        }


        public async Task<List<HubClientSession>> GetCurrentList()
        {
            var method = nameof(IClientStatusHub.GetCurrentList);
            return await _hub.InvokeUntilOK<List<HubClientSession>>(_conn, method);
        }


        public Task RequestClientStates()
            => _hub.Invoke(nameof(IClientStatusHub.RequestClientStates));


        public Task RequestClientState(string connectionID)
            => _hub.Invoke(nameof(IClientStatusHub.RequestClientState), connectionID);


        public Task RewriteConfigFile(string encryptedDTO, string connectionID)
            => _hub.Invoke(nameof(IClientStatusHub.RewriteConfigFile), encryptedDTO, connectionID);


        public async Task Connect()
        {
            if (_conn != null) return;
            _conn = new AuthenticHubConnection1(_cfg);
            _hub = _conn.CreateHubProxy(HUBNAME);

            _conn.StateChanged += e
                => _stateChanged?.Raise(e.NewState.ToString());

            SetClientStateListeners();

            await _conn.TryStartUntilConnected(ex => _log.Add(ex.Message));
        }


        private void SetClientStateListeners()
        {
            var method = nameof(IClientStatusHubEvents.ClientConnected);
            _hub.On<HubClientSession>(method, sess
                => _clientConnected.Raise(sess));

            method = nameof(IClientStatusHubEvents.ClientInteracted);
            _hub.On<HubClientSession>(method, sess
                => _clientInteracted.Raise(sess));

            method = nameof(IClientStatusHubEvents.ClientDisconnected);
            _hub.On<HubClientSession>(method, sess
                => _clientDisconnected.Raise(sess));
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
