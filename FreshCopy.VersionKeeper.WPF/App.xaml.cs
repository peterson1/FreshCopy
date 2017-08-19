﻿using FreshCopy.Server.Lib45.ComponentsRegistry;
using System.Windows;

namespace FreshCopy.VersionKeeper.WPF
{
    public partial class App : Application
    {
        private ServerBackbone _backbone;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _backbone       = new ServerBackbone(this);
            var win         = new MainWindow();
            win.DataContext = _backbone.ResolveMainVM();

            if (win.DataContext == null)
                this.Shutdown();
            else
                win.Show();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            _backbone?.Dispose();
            base.OnExit(e);
        }
    }
}
