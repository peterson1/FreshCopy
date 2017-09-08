using CommonTools.Lib.ns11.StringTools;
using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.ObjectModel;
using System;
using CommonTools.Lib.ns11.LoggingTools;

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
            var sess = new HubClientSession
            {
                UserAgent    = userAgent,
                ConnectionId = userAgent.SHA1ForUTF8().Substring(9),
                LastActivity = DateTime.Now,
                CurrentState = new CurrentClientState
                {
                    PublicIP = "221.123.456.789"
                }
            };

            sess.Logs.Add(new LogEntry("sample msg 1"));
            sess.Logs.Add(new LogEntry("sample msg 2"));

            List.Add(sess);
        }
    }
}
