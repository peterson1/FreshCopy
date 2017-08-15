using CommonTools.Lib.fx45.ViewModelTools;
using FreshCopy.Common.API.Configuration;

namespace FreshCopy.Client.Lib45.ViewModels
{
    public class MainCheckerWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Update Checker";


        public MainCheckerWindowVM(UpdateCheckerSettings updateCheckerSettings)
        {
            Config = updateCheckerSettings;
        }


        public UpdateCheckerSettings  Config  { get; }
    }
}
