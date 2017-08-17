using CommonTools.Lib.fx45.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    [HubName("VersionKeeperHub")]
    public class VersionKeeperHub1 : Hub
    {
        private CurrentHubClientsVM _clients;

        public VersionKeeperHub1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;

            
        }


        //public void SendMessage(string msg)
        //{
        //    Clients.All.SendMessage(msg);
        //}


        public override Task OnConnected()
        {
            _clients.Add(Context.ConnectionId);

            //var hubCtx = GlobalHost.ConnectionManager.GetHubContext<VersionKeeperHub1>();
            //var connMgr = GlobalHost.DependencyResolver.Resolve<IConnectionManager>();
            //var hubCtx  = connMgr.GetHubContext<VersionKeeperHub1>();

            //_clients.SetHubContext(hubCtx);

            //await hubCtx.Clients.All.SendMessage("from onConnect");

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _clients.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        //private void AnnounceCurrentClients()
        //{
        //    new Thread(new ThreadStart(delegate
        //    {
        //        MessageBox.Show($"_clients.Count: {_clients.Count}");
        //    }
        //    )).Start();
        //}
    }
}
