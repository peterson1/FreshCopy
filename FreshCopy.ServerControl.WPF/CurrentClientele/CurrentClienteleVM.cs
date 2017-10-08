using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.HubClients;
using System;
using System.Collections.Generic;
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
            RefreshCmd = R2Command.Async(_ => RefreshList(), 
                                         _ => !IsBusy, "Refresh List");
        }

        public ObservableCollection<GroupByPublicIP> ByPublicIPs { get; } = new ObservableCollection<GroupByPublicIP>();

        public IR2Command  RefreshCmd  { get; }


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
