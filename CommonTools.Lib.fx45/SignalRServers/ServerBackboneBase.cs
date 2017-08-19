using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRHubServers;
using Microsoft.AspNet.SignalR;
using System;
using System.Windows;

namespace CommonTools.Lib.fx45.SignalRServers
{
    public abstract class ServerBackboneBase<TMainVM> : IDisposable
        where TMainVM : class
    {
        private ILifetimeScope _scope;

        protected abstract void RegisterCustomComponents(ContainerBuilder builder, HubConfiguration hubCfg);
        protected abstract void RegisterSettingsFileInstance(ContainerBuilder builder);


        public ServerBackboneBase(Application application)
        {
            _scope = BuildAndBeginScope(application);
        }


        public TMainVM ResolveMainVM()
        {
            if (_scope.TryResolveOrAlert<TMainVM>
                                    (out TMainVM vm))
            {
                InitializeMainVM(vm, _scope);
                return vm;
            }
            else
                return null;
        }


        private ILifetimeScope BuildAndBeginScope(Application app)
        {
            SetDataTemplates(app);

            var builder = new ContainerBuilder();
            var hubCfg  = new HubConfiguration();

            RegisterCommonComponents(builder, hubCfg);
            RegisterCustomComponents(builder, hubCfg);
            RegisterSettingsFileInstance(builder);

            var scope  = builder.Build().BeginLifetimeScope();
            var webApp = scope.Resolve<ISignalRWebApp>();
            webApp.SetResolver(scope, hubCfg);

            return scope;
        }


        protected virtual void RegisterCommonComponents(ContainerBuilder b, HubConfiguration hubCfg)
        {
            b.Solo<SignalRServerToggleVM>();
            b.Solo<CurrentHubClientsVM>();
            b.Solo<CommonLogListVM>();
            b.Solo<ISignalRWebApp, SignalRWebApp1>();

            b.Hub<MessageBroadcastHub1, IMessageBroadcaster>(hubCfg);
        }


        protected virtual void SetDataTemplates(Application app)
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
                    try { _scope?.Dispose(); }
                    catch { }
                    _scope = null;
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