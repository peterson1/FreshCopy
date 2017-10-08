using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.HubServers;
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

            foreach (var ipGrp in sessions.Where(_ => _.HubName != ClientStatusHub.Name)
                                          .GroupBy(_ => _.HubClientIP))
            {
                var byIP = new GroupByPublicIP(ipGrp);

                foreach (var pcGrp in ipGrp.GroupBy(_ => _.ComputerName))
                {
                    var byPC = new GroupByPcName(pcGrp);

                    foreach (var agtGrp in pcGrp.GroupBy(_ => _.UserAgent))
                    {
                        byPC.ByAgents.Add(new GroupByUserAgent
                        {
                            UserAgent = agtGrp.Key,
                            Sessions  = new ObservableCollection<HubClientSession>(agtGrp)
                        });
                    }
                    byIP.ByPcNames.Add(byPC);
                }

                grpdList.Add(byIP);
            }
        }
    }
}
