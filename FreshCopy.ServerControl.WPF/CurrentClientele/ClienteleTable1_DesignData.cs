using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    class ClienteleTable1_DesignData : ObservableCollection<GroupByPublicIP>
    {
        public ClienteleTable1_DesignData()
        {
            var path     = @"FreshCopy.ServerControl.WPF\CurrentClientele\sampleSessions1.json";
            var sessions = JsonFile.Read<List<HubClientSession>>(path);
            this.FillWith(sessions);
        }
    }

    static class ListOfGroupByPublicIpExtensions
    {
        public static void FillWith(this ObservableCollection<GroupByPublicIP> grpdList, List<HubClientSession> sessions)
        {
            grpdList.Clear();

            foreach (var ipGrp in sessions.GroupBy(_ => _.HubClientIP))
            {
                var byIP = new GroupByPublicIP();
                byIP.PublicIP = ipGrp.Key;

                foreach (var pcGrp in ipGrp.GroupBy(_ => _.ComputerName))
                    byIP.ByPcNames.Add(new GroupByPcName
                    {
                        PcName = pcGrp.Key,
                        Sessions = pcGrp.ToList()
                    });

                grpdList.Add(byIP);
            }
        }
    }
}
