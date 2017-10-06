using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.UserControls.AppUpdateNotifiers;
using CommonTools.Lib.fx45.UserControls.LogLists;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Client.Lib45.HubClientProxies;
using FreshCopy.Common.API.HubClients;
using FreshCopy.ServerControl.WPF.Configuration;
using FreshCopy.ServerControl.WPF.CurrentClientele;
using System;
using System.Windows;

namespace FreshCopy.ServerControl.WPF
{
    public class Components
    {
        private static ILifetimeScope BuildScope(Application app)
        {
            SetDataTemplates(app);

            var b   = new ContainerBuilder();
            var cfg = ServerControlCfgFile.LoadOrDefault();
            b.RegisterInstance<ServerControlSettings>(cfg)
                            .As<IHubClientSettings>()
                            .AsSelf();

            b.MainWindow<MainWindowVM>();

            b.Solo<CurrentClienteleVM>();
            b.Solo<IHubSessionsClient, ClientStatusHubProxy1>();


            //  Commons
            //
            b.Solo<AppUpdateNotifierVM>();
            b.Solo<SharedLogListVM>();

            return b.Build().BeginLifetimeScope();

        }


        private static void SetDataTemplates(Application app)
        {
            if (app == null) return;
            app.SetTemplate<AppUpdateNotifierVM, AppUpdateNotifierUI>();
            app.SetTemplate<CurrentClienteleVM , CurrentClienteleUI1>();
            app.SetTemplate<SharedLogListVM, LogListUI1>();
        }


        public static void Launch<T>(Application app) where T : Window, new() { try
        {
            BuildScope(app).ShowMainWindow<T>();
        }
        catch (Exception ex) { ex.ShowAlert(true, true); }}
    }
}
