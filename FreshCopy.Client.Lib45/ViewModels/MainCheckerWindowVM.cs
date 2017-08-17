using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using FreshCopy.Client.Lib45.HubClientProxies;
using FreshCopy.Common.API.Configuration;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class MainCheckerWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Update Checker";

        private VersionKeeperClientProxy1 _client;

        public MainCheckerWindowVM(UpdateCheckerSettings updateCheckerSettings,
                                   VersionKeeperClientProxy1 versionKeeperClientProxy1)
        {
            Config  = updateCheckerSettings;
            _client = versionKeeperClientProxy1;

            ConnectCmd = R2Command.Async(_client.Connect);
            ConnectCmd.ExecuteIfItCan();
        }


        public IR2Command ConnectCmd { get; }

        public UpdateCheckerSettings  Config  { get; }


        protected override async Task BeforeExitApp()
        {
            StartBeingBusy("Disconnecting client ...");
            _client.Disconnect();
            await Task.Delay(1000);
        }
    }
}
