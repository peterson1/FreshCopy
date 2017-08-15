using Autofac;
using System.Windows;
using System;
using CommonTools.Lib.fx45.ExceptionTools;
using FreshCopy.Server.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using CommonTools.Lib.ns11.SignalRHubServers;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using System.Reflection;
using Autofac.Integration.SignalR;
using FreshCopy.Server.Lib45.ViewModels;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Server.Lib45.ViewModels.SoloFileWatcher;

namespace FreshCopy.Server.Lib45.ComponentsRegistry
{
    public class VersionKeeperComponents
    {
        private static ILifetimeScope BuildAndBeginScope(Application app)
        {
            SetDataTemplates(app);
            var b = new ContainerBuilder();

            b.Solo <MainVersionKeeperWindowVM>();

            b.Multi<IThrottledFileWatcher, ThrottledFileWatcher1>();
            b.Multi<SoloFileWatcherVM>();

            b.Solo <SignalRServerToggleVM>();
            b.Solo <CommonLogListVM>();
            b.Solo <ISignalRWebApp, SignalRWebApp1>();

            var cfg = VersionKeeperCfgFile.LoadOrDefault();
            b.RegisterInstance<VersionKeeperSettings>(cfg)
                            .As<ISignalRServerSettings>()
                            .AsSelf();

            b.RegisterHubs(Assembly.GetExecutingAssembly());

            var scope  = b.Build().BeginLifetimeScope();
            var webApp = scope.Resolve<ISignalRWebApp>();
            webApp.SetResolver(scope);

            return scope;
        }


        private static void SetDataTemplates(Application app)
        {
            //app?.SetTemplate<SoloFileWatcherVM, SoloFileWatcherUI>();
        }


        public static ILifetimeScope Build(Application app)
        {
            try
            {
                return BuildAndBeginScope(app);
            }
            catch (Exception ex)
            {
                ex.ShowAlert(true, true);
                return null;
            }
        }
    }
}
