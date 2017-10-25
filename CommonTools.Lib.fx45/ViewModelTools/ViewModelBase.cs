using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.EventHandlerTools;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private      EventHandler<string> _statusChanged;
        public event EventHandler<string>  StatusChanged
        {
            add    { _statusChanged -= value; _statusChanged += value; }
            remove { _statusChanged -= value; }
        }

        private      EventHandler _activateRequested;
        public event EventHandler  ActivateRequested
        {
            add    { _activateRequested -= value; _activateRequested += value; }
            remove { _activateRequested -= value; }
        }

        private      EventHandler _closeRequested;
        public event EventHandler  CloseRequested
        {
            add    { _closeRequested -= value; _closeRequested += value; }
            remove { _closeRequested -= value; }
        }


        public string   Title     { get; private set; }
        public bool     IsBusy    { get; private set; }
        public string   BusyText  { get; private set; }
        public string   Status    { get; private set; }


        protected virtual void UpdateTitle(string text) => Title = text;


        protected void SetStatus(string status)
        {
            AsUI(_ =>
            {
                Status = status;
                _statusChanged?.Raise(Status);
            });
        }


        protected void StartBeingBusy(string message)
        {
            AsUI(_ =>
            {
                IsBusy = true;
                BusyText = message;
            });
        }


        protected async Task StartBeingBusyAsync(string message)
        {
            await Task.Delay(1);
            StartBeingBusy(message);
            await Task.Delay(1);
        }


        protected void StopBeingBusy() => IsBusy = false;


        public void ActivateUI() => AsUI(_ => _activateRequested.Raise());
        public void CloseUI   () => AsUI(_ => _closeRequested   .Raise());


        public void AsUI(SendOrPostCallback action)
            => UIThread.Run(() => action.Invoke(null));
    }
}
