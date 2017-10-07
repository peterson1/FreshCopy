using CommonTools.Lib.fx45.ViewModelTools;
using FreshCopy.ServerControl.WPF.CurrentClientele;
using System.Threading.Tasks;

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


        protected override async Task OnWindowLoadAsync()
        {
            await Clientele.RefreshList(true);
        }


        protected override async Task OnWindowCloseAsync()
        {
            StartBeingBusy("Disconnecting from server ...");
            await Task.Delay(100);
            Clientele.Dispose();
            await Task.Delay(1000 * 2);
        }
    }
}
