using System;

namespace CommonTools.Lib.ns11.SignalRClients
{
    public class HubClientSession
    {
        public string     HubName        { get; set; }
        public string     UserAgent      { get; set; }
        public string     ConnectionId   { get; set; }
        public DateTime   LastActivity   { get; set; }

        public CurrentClientState CurrentState { get; set; }
    }
}
