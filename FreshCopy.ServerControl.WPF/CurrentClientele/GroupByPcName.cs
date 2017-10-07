using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class GroupByPcName
    {
        public string PcName { get; set; }
        public List<HubClientSession> Sessions { get; set; } = new List<HubClientSession>();
    }
}
