using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.HubClients;
using System.Collections.ObjectModel;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class CurrentClienteleVM : ViewModelBase
    {
        private IHubSessionsClient _hub;


        public CurrentClienteleVM(IHubSessionsClient clientStatusHubProxy1)
        {
            _hub = clientStatusHubProxy1;
        }


        public ObservableCollection<HubClientSession> Rows { get; } = new ObservableCollection<HubClientSession>();
    }
}
