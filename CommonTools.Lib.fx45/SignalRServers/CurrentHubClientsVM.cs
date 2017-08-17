using CommonTools.Lib.fx45.ViewModelTools;
using Microsoft.AspNet.SignalR;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public class CurrentHubClientsVM : ViewModelBase
    {
        //private IHubContext _ctx;


        //public async Task SendMessage(string message)
        //{
        //    var ctx = GlobalHost.ConnectionManager.GetHubContext<VersionKeeperHub1>();
        //    await ctx.Clients.All.SendMessage(message);
        //}


        public ObservableCollection<string> List { get; } = new ObservableCollection<string>();


        public void Add(string connectionId)
        {
            if (List.Contains(connectionId)) return;
            AsUI(_ => List.Add(connectionId));
        }


        public void Remove(string connectionId)
        {
            while (List.Contains(connectionId))
            {
                AsUI(_ => List.Remove(connectionId));
            }
        }


        //public void SetHubContext(IHubContext hubCtx) 
        //    => _ctx = hubCtx;

        public int Count => List.Count;
    }
}
