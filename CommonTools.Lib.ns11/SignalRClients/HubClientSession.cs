﻿using System;
using System.ComponentModel;

namespace CommonTools.Lib.ns11.SignalRClients
{
    public class HubClientSession : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string     HubName         { get; set; }
        public string     UserAgent       { get; set; }
        public string     AgentVersion    { get; set; }
        public string     ComputerName    { get; set; }
        public string     ConnectionId    { get; set; }
        public DateTime   LastActivity    { get; set; }
        public string     LastHubMethod   { get; set; }

        public CurrentClientState CurrentState { get; set; }
    }
}
