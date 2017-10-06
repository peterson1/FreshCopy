using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.HubClientStates;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.ScreenshotTools
{
    public class ScreenshotSenderVM : ViewModelBase
    {
        private IMessageBroadcastClient _client;
        private ClientStateComposer1      _composr;


        public ScreenshotSenderVM(IMessageBroadcastClient messageBroadcastListener,
                                  ClientStateComposer1 clientStateComposer1)
        {
            _client  = messageBroadcastListener;
            _composr = clientStateComposer1;
            SendScreenshotCmd = R2Command.Async(SendScreenshot, _ => !IsBusy, "Send Screenshot");
            SetStatus("Send Screenshot");
        }


        public IR2Command  SendScreenshotCmd  { get; }


        private async Task SendScreenshot()
        {
            StartBeingBusy("Sending Screenshot ...");
            SetStatus     ("Sending Screenshot ...");

            var state = await _composr.GatherClientState();
            _client.SendClientState(state);

            SetStatus("Screenshot sent.");
            await Task.Delay(1000 * 3);
            SetStatus("Send Screenshot");
            StopBeingBusy();
        }
    }
}
