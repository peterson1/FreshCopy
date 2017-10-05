using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.ExceptionTools;
using CommonTools.Lib.fx45.UserControls.AppUpdateNotifiers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.ServerControl.WPF.Configuration;
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


            //  Commons
            //
            b.Solo<AppUpdateNotifierVM>();

            return b.Build().BeginLifetimeScope();

        }


        private static void SetDataTemplates(Application app)
        {
            app?.SetTemplate<AppUpdateNotifierVM, AppUpdateNotifierUI>();
        }


        public static void Launch<T>(Application app) where T : Window, new() { try
        {
            BuildScope(app).ShowMainWindow<T>(true);
        }
        catch (Exception ex) { ex.ShowAlert(true, true); }}
    }
}
