﻿using Autofac;
using Autofac.Integration.SignalR;
using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.UserControls.CurrentHubClients;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Windows;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public abstract class ServerBackboneBase<TMainVM> : IDisposable
        where TMainVM : class
    {
        private ILifetimeScope   _scope;
        private IDisposable      _webApp;
        private HubConfiguration _hubCfg;


        protected abstract void RegisterCustomComponents(ContainerBuilder builder, HubConfiguration hubCfg);
        protected abstract void RegisterSettingsFileInstance(ContainerBuilder builder);


        public ServerBackboneBase(Application application)
        {
            _scope = BuildAndBeginScope(application);
        }


        public TMainVM ResolveMainVM()
        {
            if (_scope.TryResolveOrAlert<TMainVM>(out TMainVM vm))
            {
                InitializeMainVM(vm, _scope);
                return vm;
            }
            else
                return null;
        }


        private ILifetimeScope BuildAndBeginScope(Application app)
        {
            SetCommonDataTemplates(app);
            SetCustomDataTemplates(app);

            var builder = new ContainerBuilder();
            _hubCfg     = new HubConfiguration();
            _hubCfg.EnableDetailedErrors = true;

            RegisterCommonComponents(builder, _hubCfg);
            RegisterCustomComponents(builder, _hubCfg);
            RegisterSettingsFileInstance(builder);

            var scope = builder.Build().BeginLifetimeScope();
            SetServerToggleActions(scope);
            SetGlobalServerConfig(scope);

            return scope;
        }


        private void SetGlobalServerConfig(ILifetimeScope scope)
        {
            GlobalServer.Settings = scope.Resolve<ISignalRServerSettings>();
        }


        private void SetServerToggleActions(ILifetimeScope scope)
        {
            var togl = scope.Resolve<SignalRServerToggleVM>();
            togl.StartAction = url =>
            {
                _webApp = WebApp.Start(url, OnWebAppStartup);
            };
            togl.StopAction = OnWebAppStop;
        }


        private void OnWebAppStartup(IAppBuilder appBuildr)
        {
            _hubCfg.Resolver = new AutofacDependencyResolver(_scope);
            GlobalHost.DependencyResolver = _hubCfg.Resolver;

            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), 
            //    () => _scope.Resolve<IUserIdProvider>());

            appBuildr.UseAutofacMiddleware(_scope);
            appBuildr.MapSignalR("/signalr", _hubCfg);
            //GlobalHost.HubPipeline.RequireAuthentication();
        }


        private void OnWebAppStop()
        {
            try { _webApp?.Dispose(); }
            catch { }
            _webApp = null;
        }


        protected virtual void RegisterCommonComponents(ContainerBuilder b, HubConfiguration hubCfg)
        {
            b.Solo<SignalRServerToggleVM>();
            b.Solo<CurrentHubClientsVM>();
            b.Solo<SharedLogListVM>();
            //b.Solo<IUserIdProvider, Auth1UserIdProvider>();

            b.Hub<MessageBroadcastHub1, IMessageBroadcaster>(hubCfg);
        }


        private void SetCommonDataTemplates(Application app)
        {
            app.SetTemplate<CurrentHubClientsVM, CurrentHubClientsUI1>();
        }


        protected virtual void SetCustomDataTemplates(Application app)
        {
        }


        protected virtual void InitializeMainVM(TMainVM mainVM, ILifetimeScope scope)
        {
        }


        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnWebAppStop();
                    try { _scope?.Dispose(); }
                    catch { }
                    _hubCfg = null;
                    _scope  = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}