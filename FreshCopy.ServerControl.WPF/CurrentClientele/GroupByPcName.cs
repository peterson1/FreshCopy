using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class GroupByPcName
    {
        public GroupByPcName(IGrouping<string, HubClientSession> grp)
        {
            PcName = GetCompositePcName(grp);
        }


        public string PcName { get; }
        public ObservableCollection<GroupByUserAgent> ByAgents { get; set; } = new ObservableCollection<GroupByUserAgent>();


        private string GetCompositePcName(IGrouping<string, HubClientSession> grp)
            => grp.Key + "  --  local: " + grp.First().CurrentState?.PrivateIPs;
    }
}
