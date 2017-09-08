using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.SignalRClients;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class StateRequestBroadcastHandler
    {
        private IMessageBroadcastListener _listnr;
        private bool _isBusy;


        public StateRequestBroadcastHandler(IMessageBroadcastListener messageBroadcastListener)
        {
            _listnr = messageBroadcastListener;
            _listnr.BroadcastReceived += OnBroadcastReceived;
        }


        private void OnBroadcastReceived(object sender, KeyValuePair<string, string> msg)
        {
            if (!IsRelevantMessage(msg.Value)) return;
            if (_isBusy) return;

            _isBusy = true;
            Task.Run(async () =>
            {
                var state = await GatherClientState();
                await _listnr.SendClientState(state);
                _isBusy = false;
            });
        }


        private async Task<CurrentClientState> GatherClientState()
        {
            var state      = new CurrentClientState();
            state.PublicIP = await GetPublicIP();
            return state;
        }


        private async Task<string> GetPublicIP()
        {
            var httpClient = new HttpClient();
            try
            {
                return await httpClient.GetStringAsync("https://api.ipify.org");
            }
            catch (Exception ex)
            {
                return ex.Info(true, true);
            }
        }


        private bool IsRelevantMessage(string message)
            => message == typeof(CurrentClientState).Name;
    }
}
