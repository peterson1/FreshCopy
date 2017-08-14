using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public class SignalRWebApp1 : ISignalRWebApp
    {
        private        IDisposable    _webApp;
        private static ILifetimeScope _scope;


        public void StartServer(string serverUrl)
        {
            _webApp = WebApp.Start<SignalRWebApp1>(serverUrl);
        }


        public void StopServer()
        {
            try { _webApp?.Dispose(); }
            catch { }
            _webApp = null;
            try { _scope?.Dispose(); }
            catch { }
            _scope = null;
        }


        //http://docs.autofac.org/en/latest/integration/signalr.html#owin-integration
        public void Configuration(IAppBuilder app)
        {
            var hubCfg = new HubConfiguration();
            hubCfg.EnableDetailedErrors = true;

            hubCfg.Resolver = new AutofacDependencyResolver(_scope);
            app.UseAutofacMiddleware(_scope);

            app.MapSignalR("/signalr", hubCfg);
        }


        public void SetResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }


        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopServer();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
