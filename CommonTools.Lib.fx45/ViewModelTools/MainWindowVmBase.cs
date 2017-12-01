using Autofac;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
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


        protected void StartBeingBusy(string message)
        {
            IsBusy = true;
            BusyText = message;
        }

        protected void StopBeingBusy() => IsBusy = false;





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
            //var caption = taskDescription.IsBlank()
            //            ? ex?.Message
            //            : $"Error on “{taskDescription}”";

            //new Thread(new ThreadStart(delegate
            //{
            //    MessageBox.Show(ex?.Info(true, true), $"   {caption}", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //)).Start();
            Alert.Show(ex, taskDescription);
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
