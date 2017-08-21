using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.ObjectModel;
using System.Linq;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public class CurrentHubClientsVM : ViewModelBase
    {


        public ObservableCollection<HubClientSession> List { get; } = new ObservableCollection<HubClientSession>();


        public void Add(string connectionId, HubClientSession session)
        {
            Remove(connectionId);
            session.ConnectionId = connectionId;
            AsUI(_ => List.Add(session));
        }


        public void Remove(string connectionId)
        {
            while (List.Any(_ => _.ConnectionId == connectionId))
            {
                var match = List.FirstOrDefault(_ => _.ConnectionId == connectionId);
                if (match != null)
                    AsUI(_ => List.Remove(match));
            }
        }


        public int Count => List.Count;
    }
}
