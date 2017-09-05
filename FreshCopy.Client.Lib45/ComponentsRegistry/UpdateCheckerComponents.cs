using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.SignalRClients;
using CommonTools.Lib.fx45.UserControls.LogLists;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
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
        private static ILifetimeScope BuildScope(Application app)
        {
            SetDataTemplates(app);

            var b   = new ContainerBuilder();
            var cfg = UpdateCheckerCfgFile.LoadOrDefault();
            b.RegisterInstance<UpdateCheckerSettings>(cfg)
                            .As<IHubClientSettings>()
                            .AsSelf();

            b.MainWindow<MainCheckerWindowVM>();

            b.Multi<BinaryFileChangeBroadcastHandlerVM>();
            b.Multi<AppendOnlyDbChangeBroadcastHandlerVM>();
            b.Multi<IBinaryFileUpdater, BackupKeepingFileUpdater>();
            b.Multi<IAppendOnlyDbUpdater, AppendOnlyDbUpdater1>();
            b.Multi<IVersionKeeperClient, VersionKeeperClientProxy1>();


            //  Commons
            //
            b.Solo<IMessageBroadcastListener, MessageBroadcastListener1>();
            b.Solo<StateRequestBroadcastHandler>();
            b.Solo<SharedLogListVM>();
            b.Multi<ContextLogListVM>();

            return b.Build().BeginLifetimeScope();
        }


        private static void SetDataTemplates(Application app)
        {
            if (app == null) return;
            app.SetTemplate<BinaryFileChangeBroadcastHandlerVM, BinaryFileBroadcastHandlerUI>();
            app.SetTemplate<AppendOnlyDbChangeBroadcastHandlerVM, AppendOnlyDbBroadcastHandlerUI>();
            app.SetTemplate<SharedLogListVM, LogListUI1>();
            app.SetTemplate<ContextLogListVM, LogListUI1>();
        }


        public static void Launch<T>(Application app) where T : Window, new() { try
        {
            BuildScope(app).ShowMainWindow<T>(true);
        }
        catch (Exception ex) { ex.ShowAlert(true, true); }}
    }
}