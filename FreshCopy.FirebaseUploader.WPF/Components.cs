using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.GoogleTools;
using CommonTools.Lib.ns11.LoggingTools;
using FreshCopy.FirebaseUploader.WPF.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FreshCopy.FirebaseUploader.WPF
{
    class Components
    {
        private static ILifetimeScope BuildScope(Application app)
        {
            SetDataTemplates(app);

            var b = new ContainerBuilder();
            RegisterConfigs(ref b);

            b.MainWindow<MainWindowVM>();
            b.Solo<ConfigLoaderVM>();
            b.Solo<FirebaseConnection>();

            return b.Build().BeginLifetimeScope();
        }


        private static void RegisterConfigs(ref ContainerBuilder b)
        {
            var cfgs = ConfigLoaderVM.LoadAll();

            b.RegisterInstance<List<UploaderSettings>>(cfgs);

            if (!cfgs.Any()) return;
            var cfg = cfgs.First();

            b.RegisterInstance<UploaderSettings>(cfg);
            b.RegisterInstance<FirebaseCredentials>(cfg.FirebaseCreds);

            Loggly.Initialize("FirebaseUploader", cfg.LogglyToken);
        }


        private static void SetDataTemplates(Application app)
        {
            app?.SetTemplate<ConfigLoaderVM, ConfigPickerUI>();
            app?.SetTemplate<UploaderSettings, ConfigViewerUI>();
        }


        internal static void Launch<T>(App app) where T : Window, new()
        {
            T win = null; try
            {
                win = BuildScope(app).ShowMainWindow<T>();
                win.Show();
            }
            catch (Exception ex) { Alert.Show(ex); }
            if (win == null) app.Shutdown();
        }
    }
}
