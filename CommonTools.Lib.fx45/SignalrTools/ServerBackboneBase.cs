﻿using Autofac;
using Autofac.Integration.SignalR;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.UserControls.CurrentHubClients;
using CommonTools.Lib.fx45.UserControls.LogLists;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CommonTools.Lib.fx45.SignalrTools
{
    public abstract class ServerBackboneBase<TMainVM, TCfg> : IDisposable
        where TMainVM : class
        where TCfg : class, ISignalRServerSettings
    {
        private ILifetimeScope   _scope;
        private IDisposable      _webApp;
        private HubConfiguration _hubCfg;


        //protected abstract void  RegisterSettingsFileInstance (ContainerBuilder builder);
        protected abstract void  RegisterCustomComponents (ContainerBuilder builder, HubConfiguration hubCfg);
        protected abstract TCfg  GetConfigInstance        ();


        public ServerBackboneBase(Application application)
        {
            _scope = BuildAndBeginScope(application);
        }


        public TMainVM ResolveMainVM()
        {
            if (_scope.TryResolveOrAlert<TMainVM>(out TMainVM vm))
            {
                try
                {
                    InitializeMainVM(vm, _scope);
                    return vm;
                }
                catch (Exception ex)
                {
                    Alert.Show(ex, "Initialize Main VM");
                }
            }
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


        private void RegisterSettingsFileInstance(ContainerBuilder builder)
        {
            TCfg cfg = null; try
            {
                cfg = GetConfigInstance();
            }
            catch (Exception ex) { ex.ShowAlert(); }

            builder.RegisterInstance<TCfg>(cfg)
                   .As<ISignalRServerSettings>()
                   .AsSelf();
        }


        private void SetGlobalServerConfig(ILifetimeScope scope)
        {
            GlobalServer.Settings = scope.Resolve<ISignalRServerSettings>();
        }


        private void SetServerToggleActions(ILifetimeScope scope)
        {
            var togl = scope.Resolve<SignalrServerToggleVM>();
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

            SetDisconnectTimeout();

            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;

            appBuildr.UseAutofacMiddleware(_scope);
            appBuildr.MapSignalR("/signalr", _hubCfg);

            //GlobalHost.HubPipeline.RequireAuthentication();
            
            var hubPipeline = _hubCfg.Resolver.Resolve<IHubPipeline>();
            //hubPipeline.AddModule(_scope.Resolve<LoggerPipeline1>());
            foreach (var module in GetHubPipelineModules(_scope))
                hubPipeline.AddModule(module);
        }


        protected virtual IEnumerable<IHubPipelineModule> GetHubPipelineModules(ILifetimeScope scope)
            => new List<IHubPipelineModule>();


        private void SetDisconnectTimeout()
        {
            var fcpCfg = _scope.Resolve<ISignalRServerSettings>();

            var hrs = fcpCfg.DisconnectTimeoutHrs == 0
               ? 23 : fcpCfg.DisconnectTimeoutHrs;

            GlobalHost.Configuration.DisconnectTimeout
                = TimeSpan.FromHours(hrs);
        }

        private void OnWebAppStop()
        {
            try { _webApp?.Dispose(); }
            catch { }
            _webApp = null;
        }


        protected virtual void RegisterCommonComponents(ContainerBuilder b, HubConfiguration hubCfg)
        {
            b.Solo<SignalrServerToggleVM>();
            b.Solo<SharedLogListVM>();
            //b.Solo<IUserIdProvider, Auth1UserIdProvider>();
        }


        private void SetCommonDataTemplates(Application app)
        {
            if (app == null) return;
            app.SetTemplate<SharedLogListVM, LogListUI1>();
            app.SetTemplate<ContextLogListVM, LogListUI1>();
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