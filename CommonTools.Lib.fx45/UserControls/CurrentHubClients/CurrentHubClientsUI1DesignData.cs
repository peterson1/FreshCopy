using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.Collections.ObjectModel;

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
                HubName      = MessageBroadcastHub1.NAME,
                UserAgent    = userAgent,
                AgentVersion = "1.0.17252.0043",
                ConnectionId = userAgent.SHA1ForUTF8().Substring(9),
                LastActivity = DateTime.Now,
                HubClientIP  = "123.456.789.1011",
                ComputerName = "FATS-SVR2",
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
