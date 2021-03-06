﻿using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.UserControls.LogLists;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.GoogleTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.BroadcastHandlers;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Client.Lib45.HubClientProxies;
using FreshCopy.Client.Lib45.HubClientStates;
using FreshCopy.Client.Lib45.ProblemReporters;
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

            b.RegisterInstance(cfg.FirebaseCreds 
                ?? new FirebaseCredentials()).AsSelf();

            Loggly.Initialize(cfg.UserAgent, cfg.LogglyToken);

            b.MainWindow<MainCheckerWindowVM>();

            b.Solo<TrayContextMenuItems>();
            b.Multi<BinaryFileChangeBroadcastHandlerVM>();
            b.Multi<AppendOnlyDbChangeBroadcastHandlerVM>();
            b.Multi<IBinaryFileUpdater, BackupKeepingFileUpdater>();
            b.Multi<IAppendOnlyDbUpdater, AppendOnlyDbUpdater1>();
            b.Multi<IVersionKeeperClient, VersionKeeperClientProxy1>();
            b.Multi<ClientStateComposer1>();
            b.Solo<StateRequestBroadcastHandler>();
            b.Solo<CfgEditorHubEventHandler>();
            b.Solo<ProblemReporter1VM>();
            b.Solo<FirebaseConnection>();
            b.Solo<AgentStateUpdater>();
            b.Solo<JobOrderWatcher>();
            b.Solo<NewVersionWatcher>();


            //  Commons
            //
            b.Solo<IMessageBroadcastClient, MessageBroadcastHubProxy1>();
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


        public static async void Launch<T>(Application app) where T : Window, new()
        {
            T win = null;
            try
            {
                win = BuildScope(app).ShowMainWindow<T>(true);
                win?.Hide();
            }
            //catch (IntrusionAttemptException) { Alert.Show("Not authorized"); }
            //catch (Exception ex) { ex.ShowAlert(true, true); }
            catch (Exception ex) { await Loggly.Post(ex); }

            if (win == null) app.Shutdown();
        }
    }
}