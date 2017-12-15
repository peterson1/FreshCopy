using CommonTools.Lib.fx45.ViewModelTools;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib.fx45.Telemetry
{
    public static class MainWindowVmExtensions
    {
        public static void SetupTelemetry(this MainWindowVmBase vm, Window win, string instrumentationKey, bool hideOnWindowClose)
        {
            AppInsights.StartTracking(instrumentationKey);
            SetAppEventHandlers();
            AppInsights.PostEvent("Telemetry started.");

            vm.HandleWindowEvents(win, null, hideOnWindowClose);
        }


        private static void SetAppEventHandlers()
        {
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                OnError(e.Exception, "Application.Current");
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                OnError(e.Exception, "TaskScheduler");
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                OnError(e.ExceptionObject as Exception, "AppDomain.CurrentDomain");
                // application terminates after above
            };

            AppDomain.CurrentDomain.ProcessExit += (s, e)
                => AppInsights.PostEvent("Telemetry ended.");
        }


        private static void OnError(Exception ex, string errorContext)
            => AppInsights.Post(ex, errorContext);
    }
}
