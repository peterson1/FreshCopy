using Autofac;
using CommonTools.Lib.fx45.DependencyInjection;
using FreshCopy.Server.Lib45.ComponentsRegistry;
using FreshCopy.Server.Lib45.ViewModels;
using System.Windows;

namespace FreshCopy.VersionKeeper.WPF
{
    public partial class App : Application
    {
        private ILifetimeScope _scope;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _scope = VersionKeeperComponents.Build(this);
            if (_scope.TryResolveOrAlert<MainVersionKeeperWindowVM>
                                    (out MainVersionKeeperWindowVM vm))
            {
                var win = new MainWindow();
                win.DataContext = vm;
                win.Show();
                vm.StartFileWatchers(_scope);
            }
            else
                this.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            try { _scope?.Dispose(); }
            catch { }
        }
    }
}
