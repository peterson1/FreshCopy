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
            AppInsights.Post("Telemetry started.");

            vm.HandleWindowEvents(win, null, hideOnWindowClose);
        }


        private static void SetAppEventHandlers()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                AppInsights.Post("Telemetry ended.");
                AppInsights.Flush();
            };
        }
    }
}
