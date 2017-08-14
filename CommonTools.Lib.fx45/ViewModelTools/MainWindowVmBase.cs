using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.ns11.InputTools;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public abstract class MainWindowVmBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        protected string _exeVer;


        public MainWindowVmBase()
        {
            _exeVer = CurrentExe.GetVersion();
            ExitCmd = R2Command.Async(ExitApp);
            AppendToCaption("...");
        }

        protected abstract string   CaptionPrefix   { get; }

        public string       Caption           { get; protected set; }
        public int          SelectedTabIndex  { get; set; }
        public IR2Command   ExitCmd           { get; }
        public bool         IsBusy            { get; private set; }
        public string       BusyText          { get; private set; }



        protected void StartBeingBusy(string message)
        {
            IsBusy = true;
            BusyText = message;
        }

        protected void StopBeingBusy() => IsBusy = false;





        private async Task ExitApp()
        {
            await BeforeExitApp();
            Application.Current.Shutdown();
        }


        protected virtual async Task BeforeExitApp()
        {
            await Task.Delay(1);
        }


        protected virtual void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {text}";
    }
}
