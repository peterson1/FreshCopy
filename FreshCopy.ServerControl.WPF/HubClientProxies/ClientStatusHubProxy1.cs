using CommonTools.Lib.fx45.SignalRClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCopy.ServerControl.WPF.HubClientProxies
{
    class ClientStatusHubProxy1
    {
        public ClientStateListener1 _listenr;


        public ClientStatusHubProxy1(ClientStateListener1 clientStateListener1)
        {
            _listenr = clientStateListener1;
        }
    }
}
