﻿using CommonTools.Lib.fx45.DependencyInjection;
using FreshCopy.Client.Lib45.ComponentsRegistry;
using FreshCopy.Client.Lib45.ViewModels;
using System.Windows;

namespace FreshCopy.UpdateChecker.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var scope = UpdateCheckerComponents.Build(this);
            
            if (scope.TryResolveOrAlert<MainCheckerWindowVM>
                                    (out MainCheckerWindowVM vm))
            {
                var win = new MainWindow();
                vm.HandleWindowEvents(win, scope);
                win.Show();
                //vm.StartBroadcastHandlers(scope);
            }
            else
                this.Shutdown();
        }
    }
}
