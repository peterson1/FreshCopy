using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.InputTools;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.InputTools
{
    public class R2AsyncCommandWPF : IR2Command, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private   string                 _origLabel;
        private   bool                   _origOverride;
        protected Predicate<object>      _canExecute;


        internal R2AsyncCommandWPF(Func<object, Task> task, Predicate<object> canExecute, string buttonLabel)
        {
            AsyncTask    = task;
            _canExecute  = canExecute;
            CurrentLabel = buttonLabel;
        }


        public string    CurrentLabel      { get; set; }
        public bool      IsBusy            { get; protected set; }
        public bool      IsCheckable       { get; set; }
        public bool      IsChecked         { get; set; }
        public bool      OverrideEnabled   { get; set; } = true;
        public bool      DisableWhenDone   { get; set; }
        public bool      LastExecutedOK    { get; protected set; }
        public DateTime  LastExecuteStart  { get; protected set; }
        public DateTime  LastExecuteEnd    { get; protected set; }

        public Func<object, Task>  AsyncTask  { get; }

        public Task RunAsync(object arg = null) => AsyncTask(arg);


        public async void Execute(object parameter)
        {
            if (IsBusy) return;
            if (!OverrideEnabled) return;

            IsBusy           = true;
            _origOverride    = OverrideEnabled;
            _origLabel       = CurrentLabel;
            CurrentLabel     = $"Running “{_origLabel}”…";
            OverrideEnabled  = false;
            LastExecuteStart = DateTime.Now;

            LastExecutedOK   = await SafeRun(parameter);

            ConcludeExecute();
            CommandManager.InvalidateRequerySuggested();
        }


        private async Task<bool> SafeRun(object parameter)
        {
            try
            {
                await AsyncTask.Invoke(parameter);
                return true;
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }
        }


        //protected virtual void OnError(Exception error)
        //    => MessageBox.Show(error.Info(false, true),
        //        $"Error on task :  “{_origLabel}”",
        //        MessageBoxButton.OK, MessageBoxImage.Error);
        protected virtual void OnError(Exception error)
            => Alert.Show(error, _origLabel);


        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter)
        {
            if (IsBusy) return false;
            if (!OverrideEnabled) return false;
            return _canExecute?.Invoke(parameter) ?? true;
        }


        public void ExecuteIfItCan(object param = null)
        {
            if (CanExecute(param)) Execute(param);
        }


        public override string ToString() => CurrentLabel;


        public void ConcludeExecute()
        {
            LastExecuteEnd  = DateTime.Now;
            IsBusy          = false;
            CurrentLabel    = _origLabel;
            OverrideEnabled = DisableWhenDone ? false : _origOverride;
        }
    }
}
