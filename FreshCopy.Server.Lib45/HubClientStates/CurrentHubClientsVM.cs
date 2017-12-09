using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Server.Lib45.SignalRHubs;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FreshCopy.Server.Lib45.HubClientStates
{
    public class CurrentHubClientsVM : ViewModelBase
    {


        public ObservableCollection<HubClientSession> List { get; } = new ObservableCollection<HubClientSession>();


        public void AddOrUpdate(HubClientSession session)
        {
            var connId = session.ConnectionId;
            var existing = List.FirstOrDefault(_ => _.ConnectionId == connId);

            if (existing == null)
                ClientStateListeners.Notify.ClientConnected(session);
            else
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
                ClientStateListeners.Notify.ClientInteracted(session);
            }
            AsUI(_ => List.Add(session));
        }




        private void ShowScreenshotIfAny(HubClientSession session)
        {
            var cap = $"[{DateTime.Now.ToShortTimeString()}]  {session.UserAgent}";
            AsUI(_ => BitmapWindow1.Show(cap, session.CurrentState?.ScreenshotB64));
        }


        private static void ConsolidateLogs(HubClientSession session, HubClientSession existing)
        {
            if (existing.Logs.Any() && !session.Logs.Any())
                session.Logs.Add(existing.Logs);

            if (existing.Errors.Any() && !session.Errors.Any())
                session.Errors.Add(existing.Errors);
        }


        public void Remove(string connectionId)
        {
            while (List.Any(_ => _.ConnectionId == connectionId))
            {
                var match = List.FirstOrDefault(_ => _.ConnectionId == connectionId);
                if (match != null)
                {
                    ClientStateListeners.Notify.ClientDisconnected(match);
                    AsUI(_ => List.Remove(match));
                } 
            }
        }


        public HubClientSession this[string connectionId]
        {
            get => List.SingleOrDefault(x => x.ConnectionId == connectionId);
        }

        public int Count => List.Count;
    }
}
