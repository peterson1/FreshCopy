using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.StringTools;
using System.Collections.ObjectModel;
using System.Linq;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class GroupByPublicIP
    {
        public GroupByPublicIP(IGrouping<string, HubClientSession> grp)
        {
            PublicIP = GetCompositePublicIp(grp);
        }


        public string PublicIP { get; }
        public ObservableCollection<GroupByPcName> ByPcNames { get; set; } = new ObservableCollection<GroupByPcName>();


        private string GetCompositePublicIp(IGrouping<string, HubClientSession> grp)
        {
            var real = grp.FirstOrDefault(_ => !(_.CurrentState?.PublicIP?.IsBlank() ?? true));
            return grp.Key + ((real == null) ? "" 
                : $"  (global: {real.CurrentState.PublicIP})");
        }
    }
}
