using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;

namespace CommonTools.Lib.fx45.ScreenshotTools
{
    public class ScreenshotSenderVM : ViewModelBase
    {
        private IMessageBroadcastListener _client;


        public ScreenshotSenderVM(IMessageBroadcastListener messageBroadcastListener)
        {
            _client = messageBroadcastListener;
            SendScreenshotCmd = R2Command.Relay(SendScreenshot, _ => !IsBusy, "Send Screenshot");
        }


        public IR2Command  SendScreenshotCmd  { get; }


        private void SendScreenshot()
        {
            var state = new CurrentClientState();
            state.ScreenshotB64 = CreateBitmap.FromPrimaryScreen()
                                              .ConvertToBase64();
            _client.SendClientState(state);
        }
    }
}
