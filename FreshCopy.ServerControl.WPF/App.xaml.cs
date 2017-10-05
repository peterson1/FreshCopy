using System.Windows;

namespace FreshCopy.ServerControl.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Components.Launch<MainWindow>(this);
        }
    }
}
