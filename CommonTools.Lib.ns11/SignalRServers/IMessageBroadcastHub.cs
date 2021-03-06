﻿using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.SignalRClients;

namespace CommonTools.Lib.ns11.SignalRServers
{
    public interface IMessageBroadcastHub
    {
        void ReceiveClientState(CurrentClientState clientState);
        void ReceiveException  (ExceptionReport exceptionReport);
    }
}
