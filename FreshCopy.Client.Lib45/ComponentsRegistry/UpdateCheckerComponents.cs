using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.SignalRClients;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using FreshCopy.Client.Lib45.BroadcastHandlers;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Client.Lib45.HubClientProxies;
using FreshCopy.Client.Lib45.TargetUpdaters;
using FreshCopy.Client.Lib45.ViewModels;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.TargetUpdaters;
using System;
using System.Windows;

namespace FreshCopy.Client.Lib45.ComponentsRegistry
{
    public class UpdateCheckerComponents
    {
        private static ILifetimeScope BuildAndBeginScope(Application app)
        {
            SetDataTemplates(app);

            var b   = new ContainerBuilder();
            var cfg = UpdateCheckerCfgFile.LoadOrDefault();
            b.RegisterInstance<UpdateCheckerSettings>(cfg)
                            .As<IHubClientSettings>()
                            .AsSelf();

            b.Solo<MainCheckerWindowVM>();

            b.Multi<BinaryFileBroadcastHandlerVM>();
            b.Multi<IBinaryFileUpdater, BackupKeepingFileUpdater>();
            b.Multi<IVersionKeeperClient, VersionKeeperClientProxy1>();


            //  Commons
            //
            b.Solo<IMessageBroadcastListener, MessageBroadcastListener1>();
            b.Solo<SharedLogListVM>();
            b.Multi<ContextLogListVM>();

            return b.Build().BeginLifetimeScope();
        }


        private static void SetDataTemplates(Application app)
        {
            app?.SetTemplate<BinaryFileBroadcastHandlerVM, BinaryFileBroadcastHandlerUI>();
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
