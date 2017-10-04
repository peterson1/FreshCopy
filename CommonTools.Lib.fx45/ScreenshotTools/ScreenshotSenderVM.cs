using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.ScreenshotTools
{
    public class ScreenshotSenderVM : ViewModelBase
    {
        public ScreenshotSenderVM()
        {
            SetStatus("Send Screenshot");
            SendScreenshotCmd = R2Command.Async(SendScreenshot, _ => !IsBusy, "Send Screenshot");
        }


        public IR2Command  SendScreenshotCmd  { get; }


        private async Task SendScreenshot()
        {
            StartBeingBusy("Capturing current screen ...");
            SetStatus("Capturing current screen ...");
            await Task.Delay(1000 * 5);
            SetStatus("Sending captured screen ...");
            await Task.Delay(1000 * 5);
            SetStatus("Screenshot sent.");
            StopBeingBusy();
            await Task.Delay(1000 * 2);
            SetStatus("Send Screenshot");
        }
    }
}
