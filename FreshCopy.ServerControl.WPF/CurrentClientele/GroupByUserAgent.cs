using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.ObjectModel;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class GroupByUserAgent
    {
        public string UserAgent { get; set; }
        public ObservableCollection<HubClientSession> Sessions { get; set; } = new ObservableCollection<HubClientSession>();
    }
}
