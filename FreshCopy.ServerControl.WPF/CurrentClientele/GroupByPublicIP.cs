using System.Collections.Generic;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class GroupByPublicIP
    {
        public string PublicIP { get; set; }
        public List<GroupByPcName> ByPcNames { get; set; } = new List<GroupByPcName>();
    }
}
