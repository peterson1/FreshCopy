using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.HubClientStates;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class StateRequestBroadcastHandler
    {
        private IMessageBroadcastClient _listnr;
        private bool                      _isBusy;
        private ClientStateComposer1      _composr;


        public StateRequestBroadcastHandler(IMessageBroadcastClient messageBroadcastListener,
                                            ClientStateComposer1 clientStateComposer1)
        {
            _composr = clientStateComposer1;
            _listnr  = messageBroadcastListener;
            _listnr.BroadcastReceived += OnBroadcastReceived;
        }


        private void OnBroadcastReceived(object sender, KeyValuePair<string, string> msg)
        {
            if (!IsRelevantMessage(msg.Value)) return;
            if (_isBusy) return;

            _isBusy = true;
            Task.Run(async () =>
            {
                var state = await _composr.GatherClientState();
                _listnr.SendClientState(state);
                _isBusy = false;
            });
        }


        private bool IsRelevantMessage(string message)
            => message == typeof(CurrentClientState).Name;
    }
}
