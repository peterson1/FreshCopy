using Autofac;
using Autofac.Integration.SignalR;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.SignalRHubServers;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using FreshCopy.Server.Lib45.FileWatchers;
using FreshCopy.Server.Lib45.ViewModels;
using System;
using System.Reflection;
using System.Windows;

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
            b.Multi<BinaryFileWatcherVM>();

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
