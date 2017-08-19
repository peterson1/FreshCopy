using Autofac;
using System.Windows;
using System;
using CommonTools.Lib.fx45.ExceptionTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using CommonTools.Lib.ns11.SignalRHubServers;
using FreshCopy.Client.Lib45.ViewModels;
using CommonTools.Lib.fx45.DependencyInjection;
using FreshCopy.Client.Lib45.HubClientProxies;
using CommonTools.Lib.fx45.SignalRClients;

namespace FreshCopy.Client.Lib45.ComponentsRegistry
{
    public class UpdateCheckerComponents
    {
        private static ILifetimeScope BuildAndBeginScope(Application app)
        {

            var b   = new ContainerBuilder();
            var cfg = UpdateCheckerCfgFile.LoadOrDefault();
            b.RegisterInstance<UpdateCheckerSettings>(cfg)
                            .As<IHubClientSettings>()
                            .AsSelf();

            b.Solo<MainCheckerWindowVM>();

            b.Solo<VersionKeeperClientProxy1>();
            b.Solo<IMessageBroadcastListener, MessageBroadcastListener1>();

            return b.Build().BeginLifetimeScope();
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
