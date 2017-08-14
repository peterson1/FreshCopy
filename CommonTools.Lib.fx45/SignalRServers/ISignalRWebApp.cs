using Autofac;
using Owin;
using System;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public interface ISignalRWebApp : IDisposable
    {
        void  StartServer   (string serverURI);
        void  StopServer    ();
              
        void  Configuration (IAppBuilder app);
        void  SetResolver   (ILifetimeScope scope);
    }
}
