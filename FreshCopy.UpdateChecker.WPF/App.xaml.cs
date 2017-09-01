using FreshCopy.Client.Lib45.ComponentsRegistry;
using System.Windows;

namespace FreshCopy.UpdateChecker.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            UpdateCheckerComponents.Launch<MainWindow>(this);
        }
    }
}
