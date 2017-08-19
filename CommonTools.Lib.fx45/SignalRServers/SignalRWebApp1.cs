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
        private        IDisposable      _webApp;
        private static ILifetimeScope   _scope;
        private static HubConfiguration _hubCfg;


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


        public void Configuration(IAppBuilder app)
        {
            //  from Autofac:
            //    http://docs.autofac.org/en/latest/integration/signalr.html#owin-integration
            //
            //var hubCfg = new HubConfiguration();
            //hubCfg.EnableDetailedErrors = true;

            //hubCfg.Resolver = new AutofacDependencyResolver(_scope);
            //app.UseAutofacMiddleware(_scope);

            //app.MapSignalR("/signalr", hubCfg);


            //DoMethod2(app);
            DoMethod3(app);

            //var hubPipeline = autofacResolvr.Resolve<IHubPipeline>();
            //hubPipeline.AddModule(_scope.Resolve<LoggerPipeline1>());


        }


        //  from SO:
        //    https://stackoverflow.com/a/21126852/3973863
        ///
        private static void DoMethod2(IAppBuilder app)
        {
            var autofacResolvr = new AutofacDependencyResolver(_scope);
            GlobalHost.DependencyResolver = autofacResolvr;

            app.MapSignalR();
        }


        //  alternative:
        //    https://stackoverflow.com/a/36476106/3973863
        private static void DoMethod3(IAppBuilder app)
        {
            _hubCfg.Resolver = new AutofacDependencyResolver(_scope);
            GlobalHost.DependencyResolver = _hubCfg.Resolver;
            app.UseAutofacMiddleware(_scope);
            app.MapSignalR("/signalr", _hubCfg);
        }


        public void SetResolver(ILifetimeScope scope, HubConfiguration hubConfiguration)
        {
            _scope  = scope;
            _hubCfg = hubConfiguration;
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
