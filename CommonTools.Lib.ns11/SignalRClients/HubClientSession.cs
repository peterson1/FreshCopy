using System;

namespace CommonTools.Lib.ns11.SignalRClients
{
    public class HubClientSession
    {
        public string    ConnectionId  { get; set; }
        public string    UserAgent     { get; set; }
        public DateTime  LastActivity  { get; set; }
    }
}
