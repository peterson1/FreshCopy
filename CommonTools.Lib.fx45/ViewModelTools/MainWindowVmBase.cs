using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.ComponentModel;
using System.Threading;
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


        public void HandleWindowEvents(Window win)
        {
            SetGlobalErrorHandlers();

            win.Closing += async (s, e) =>
            {
                e.Cancel = true;
                var vm = win.DataContext as MainWindowVmBase;
                await vm.ExitCmd.RunAsync();
            };

            win.DataContext = this;
            Task.Run(async () =>
            {
                try
                {
                    OnWindowLoad();
                    await OnWindowLoadAsync();
                }
                catch (Exception ex)
                {
                    OnError(ex, "Window Initialize");
                }
            });
        }



        protected void StartBeingBusy(string message)
        {
            IsBusy = true;
            BusyText = message;
        }

        protected void StopBeingBusy() => IsBusy = false;





        private async Task ExitApp()
        {
            OnWindowClose();
            await OnWindowCloseAsync();
            Application.Current.Shutdown();
        }


        protected virtual void OnWindowLoad()
        {
        }


        protected virtual async Task OnWindowLoadAsync()
        {
            await Task.Delay(1);
        }


        protected virtual void OnWindowClose()
        {
        }


        protected virtual async Task OnWindowCloseAsync()
        {
            await Task.Delay(1);
        }


        protected virtual void OnError(Exception ex, string taskDescription = null)
        {
            var caption = taskDescription.IsBlank()
                        ? ex?.Message
                        : $"Error on “{taskDescription}”";

            new Thread(new ThreadStart(delegate
            {
                MessageBox.Show(ex?.Info(true, true), $"   {caption}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            )).Start();
        }


        protected virtual void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {text}";


        private void SetGlobalErrorHandlers()
        {
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                OnError(e.Exception, "Application.Current");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                OnError(e.Exception, "TaskScheduler");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                OnError(e.ExceptionObject as Exception, "AppDomain.CurrentDomain");
                // application terminates after above
            };
        }
    }
}
