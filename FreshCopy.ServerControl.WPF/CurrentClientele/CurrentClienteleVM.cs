using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.HubClients;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class CurrentClienteleVM : ViewModelBase, IDisposable
    {
        private IHubSessionsClient _hub;


        public CurrentClienteleVM(IHubSessionsClient clientStatusHubProxy1)
        {
            _hub = clientStatusHubProxy1;
            _hub.ClientConnected    += (s, e) => RefreshWhenNotBusy();
            _hub.ClientInteracted   += (s, e) => RefreshWhenNotBusy();
            _hub.ClientDisconnected += (s, e) => RefreshWhenNotBusy();

            GetCurrentListCmd = R2Command.Async(_ => RefreshList(), 
                                                _ => !IsBusy, "Refresh List");
            RequestStatesCmd = R2Command.Async(RequestStates, _ => !IsBusy, "Request States");
        }


        public ObservableCollection<GroupByPublicIP> ByPublicIPs { get; } = new ObservableCollection<GroupByPublicIP>();

        public IR2Command  GetCurrentListCmd  { get; }
        public IR2Command  RequestStatesCmd   { get; }


        public async Task RefreshList(bool connectBeforeQuery = false)
        {
            StartBeingBusy("Getting Clients list ...");

            if (connectBeforeQuery) await _hub.Connect();
            var sessions = await _hub.GetCurrentList();

            //await Task.Delay(0);
            //var path = @"..\..\CurrentClientele\sampleSessions1.json";
            //var sessions = JsonFile.Read<List<HubClientSession>>(path);

            AsUI(_ => ByPublicIPs.FillWith(sessions));
            StopBeingBusy();
        }


        private async Task RequestStates()
        {
            StartBeingBusy("Requesting client states ...");
            await _hub.RequestClientStates();
            StopBeingBusy();
        }


        private void RefreshWhenNotBusy() => 
            Task.Run(async () =>
            {
                while (IsBusy)
                    await Task.Delay(500);

                GetCurrentListCmd.ExecuteIfItCan();
            });


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _hub?.Dispose();
                    _hub = null;
                }
                disposedValue = true;
            }
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
