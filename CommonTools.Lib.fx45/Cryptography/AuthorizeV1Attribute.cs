using CommonTools.Lib.ns11.SignalRClients;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace CommonTools.Lib.fx45.Cryptography
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AuthorizeV1Attribute : AuthorizeAttribute
    {
        public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
        {
            return request.TryGetSession(out HubClientSession usr);
        }
    }
}
