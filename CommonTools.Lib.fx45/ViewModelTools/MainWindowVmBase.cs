using Autofac;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.Telemetry;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.DependencyInjection;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public abstract partial class MainWindowVmBase : INotifyPropertyChanged, IComponentResolver
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private      EventHandler _onWindowCloseRequested;
        public event EventHandler  OnWindowCloseRequested
        {
            add    { _onWindowCloseRequested -= value; _onWindowCloseRequested += value; }
            remove { _onWindowCloseRequested -= value;}
        }

        private      EventHandler _onWindowHidden;
        public event EventHandler  OnWindowHidden
        {
            add    { _onWindowHidden -= value; _onWindowHidden += value; }
            remove { _onWindowHidden -= value;}
        }



        protected string       _exeVer;
        private ILifetimeScope _scope;


        public MainWindowVmBase()
        {
            _exeVer     = CurrentExe.GetVersion();
            ExitCmd     = R2Command.Async(_ => ExitApp(false));
            RelaunchCmd = R2Command.Async(_ => ExitApp(true));
            AppendToCaption("...");
        }

        protected abstract string   CaptionPrefix   { get; }

        public string       Caption           { get; protected set; }
        public int          SelectedTabIndex  { get; set; }
        public IR2Command   RelaunchCmd       { get; }
        public IR2Command   ExitCmd           { get; }
        public bool         IsBusy            { get; private set; }
        public string       BusyText          { get; private set; }


        public void StartBeingBusy(string message)
        {
            IsBusy = true;
            BusyText = message;
        }

        public void StopBeingBusy() => IsBusy = false;



        public void RequestWindowClose()
            => _onWindowCloseRequested.Invoke(null, EventArgs.Empty);


        private async Task ExitApp(bool relaunchAfter)
        {
            try
            {
                OnWindowClose();
                await OnWindowCloseAsync();
                _scope?.Dispose();
                //Application.Current.Shutdown();
                if (relaunchAfter)
                    CurrentExe.RelaunchApp();
                else
                    CurrentExe.Shutdown();
            }
            catch { }
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
            AppInsights.Post(ex, taskDescription);
            Alert.Show(ex, taskDescription);
            //await Loggly.Post(ex);
        }


        protected virtual void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {text}";


        public T Resolve<T>() where T : class
        {
            if (_scope == null)
                throw new NullReferenceException("_scope == NULL"
                    + L.f + "Pass the scope in HandleWindowEvents before calling Resolve");

            return _scope.Resolve<T>();
        }


        public void AsUI(SendOrPostCallback action)
            => UIThread.Run(() => action.Invoke(null));
    }
}
