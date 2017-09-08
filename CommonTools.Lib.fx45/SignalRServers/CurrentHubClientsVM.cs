using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public class CurrentHubClientsVM : ViewModelBase
    {


        public ObservableCollection<HubClientSession> List { get; } = new ObservableCollection<HubClientSession>();


        public void AddOrUpdate(HubClientSession session)
        {
            var connId = session.ConnectionId;
            var existing = List.FirstOrDefault(_ => _.ConnectionId == connId);
            if (existing != null)
            {
                try
                {
                    AsUI(_ => ConsolidateLogs(session, existing));
                    Remove(connId);
                }
                catch (Exception ex)
                {
                    Alert.Show(ex, "AddOrUpdate existing client");
                }
            }
            AsUI(_ => List.Add(session));
        }


        private static void ConsolidateLogs(HubClientSession session, HubClientSession existing)
        {
            //var combined = existing.Logs.Concat(session.Logs).ToList();
            //session.Logs.Clear();
            //
            //foreach (var entry in combined)
            //    session.Logs.Add(entry);
            if (existing.Logs.Any() && !session.Logs.Any())
                session.Logs.Add(existing.Logs);
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


        public HubClientSession this[string connectionId]
        {
            get => List.SingleOrDefault(x => x.ConnectionId == connectionId);
        }

        public int Count => List.Count;
    }
}
