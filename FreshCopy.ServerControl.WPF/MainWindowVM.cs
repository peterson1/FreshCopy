using CommonTools.Lib.fx45.ViewModelTools;
using FreshCopy.ServerControl.WPF.CurrentClientele;

namespace FreshCopy.ServerControl.WPF
{
    class MainWindowVM : MainWindowVmBase
    {
        protected override string CaptionPrefix => "Fresh Copy | Server Control";


        public MainWindowVM(CurrentClienteleVM currentClienteleVM)
        {
            Clientele = currentClienteleVM;
        }


        public CurrentClienteleVM Clientele { get; }
    }
}
