using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.ServerControl.WPF.HubClientProxies;
using System.Collections.ObjectModel;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class CurrentClienteleVM : ViewModelBase
    {
        private ClientStatusHubProxy1 _hub;


        public CurrentClienteleVM(ClientStatusHubProxy1 clientStatusHubProxy1)
        {
            _hub = clientStatusHubProxy1;
        }


        public ObservableCollection<HubClientSession> Rows { get; } = new ObservableCollection<HubClientSession>();
    }
}
