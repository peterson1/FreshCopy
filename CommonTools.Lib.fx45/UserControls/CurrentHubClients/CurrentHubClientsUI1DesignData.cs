using CommonTools.Lib.ns11.StringTools;
using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.ObjectModel;
using System;

namespace CommonTools.Lib.fx45.UserControls.CurrentHubClients
{
    class CurrentHubClientsUI1DesignData
    {
        public CurrentHubClientsUI1DesignData()
        {
            Add("Balagtas Acctg PC");
            Add("Balagtas Cashier PC");
            Add("Balagtas Fats Svr");
            Add("Tabing-Ilog Acctg PC");
            Add("Tabing-Ilog Cashier PC");
            Add("Tabing-Ilog Fats Svr");
            Add("Meycauayan Acctg PC");
            Add("Meycauayan Cashier PC");
            Add("Meycauayan Fats Svr");
        }


        public ObservableCollection<HubClientSession> List { get; } = new ObservableCollection<HubClientSession>();


        private void Add(string userAgent)
        {
            List.Add(new HubClientSession
            {
                UserAgent    = userAgent,
                ConnectionId = userAgent.SHA1ForUTF8().Substring(9)
            });
        }
    }
}
