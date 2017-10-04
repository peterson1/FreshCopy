using CommonTools.Lib.fx45.SignalRServers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.Cryptography
{
    public class AuthorizeHelperV1
    {
        private CurrentHubClientsVM _clients;


        public AuthorizeHelperV1(CurrentHubClientsVM activeHubClientsList)
        {
            _clients = activeHubClientsList;
        }
    }
}
