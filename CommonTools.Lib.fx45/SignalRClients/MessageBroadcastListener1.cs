﻿using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRClients
{
    public class MessageBroadcastListener1 : IMessageBroadcastListener
    {
        private const string HUBNAME = MessageBroadcastHub1.NAME;

        private      EventHandler<KeyValuePair<string, string>> _broadcastReceived;
        public event EventHandler<KeyValuePair<string, string>>  BroadcastReceived
        {
            add    { _broadcastReceived -= value; _broadcastReceived += value; }
            remove { _broadcastReceived -= value; }
        }


        private IHubClientSettings _cfg;
        private HubConnection      _conn;
        private IHubProxy          _hub;
        private SharedLogListVM    _log;


        public MessageBroadcastListener1(IHubClientSettings hubClientSettings, SharedLogListVM sharedLogListVM)
        {
            _cfg = hubClientSettings;
            _log = sharedLogListVM;
        }


        public async Task Connect()
        {
            if (_conn != null) return;
            _conn      = new AuthenticHubConnection1(_cfg);
            _hub       = _conn.CreateHubProxy(HUBNAME);
            var method = nameof(IMessageBroadcaster.BroadcastMessage);

            _hub.On<string, string>(method, (subj, msg)
                => _broadcastReceived?.Invoke(this, new KeyValuePair<string, string>(subj, msg)));

            await _conn.TryStartUntilConnected(ex => _log.Add(ex.Message));
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
