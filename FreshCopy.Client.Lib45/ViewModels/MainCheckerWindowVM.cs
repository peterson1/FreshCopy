using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRHubServers;
using FreshCopy.Client.Lib45.HubClientProxies;
using FreshCopy.Common.API.Configuration;
using System.Threading.Tasks;
using System.Windows;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class MainCheckerWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Update Checker";

        private IMessageBroadcastListener _client;

        public MainCheckerWindowVM(UpdateCheckerSettings updateCheckerSettings,
                                   IMessageBroadcastListener messageBroadcastListener)
        {
            Config  = updateCheckerSettings;
            _client = messageBroadcastListener;

            _client.BroadcastReceived += _client_BroadcastReceived;

            ConnectCmd = R2Command.Async(_client.Connect);
            ConnectCmd.ExecuteIfItCan();
        }


        public IR2Command             ConnectCmd  { get; }
        public UpdateCheckerSettings  Config      { get; }


        private void _client_BroadcastReceived(object sender, string origMsg)
        {
            var msg = $"client got: “{origMsg}”";
            UIThread.Run(() => MessageBox.Show(msg));
        }


        protected override async Task BeforeExitApp()
        {
            StartBeingBusy("Disconnecting client ...");
            _client.Disconnect();
            await Task.Delay(1000);
        }
    }
}
