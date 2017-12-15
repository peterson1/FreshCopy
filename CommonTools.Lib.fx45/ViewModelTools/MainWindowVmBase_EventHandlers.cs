using Autofac;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.Telemetry;
using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.fx45.UIExtensions;
using CommonTools.Lib.ns11.DependencyInjection;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public abstract partial class MainWindowVmBase : INotifyPropertyChanged, IComponentResolver
    {


        public void HandleWindowEvents(Window win, 
            ILifetimeScope scope, bool hideOnWindowClose = false)
        {
            _scope = scope;

            HandleGlobalErrors    ();
            HandleWindowClosing   (win, hideOnWindowClose);
            win.MakeDraggable();

            win.DataContext = this;
            RunOnWindowLoadRoutines();
        }


        public void HandleSubWindowEvents(Window win)
        {
            win.MakeDraggable();
            win.DataContext = this;
            RunOnWindowLoadRoutines();
        }


        private void RunOnWindowLoadRoutines()
        {
            Task.Run(async () =>
            {
                try
                {
                    AppInsights.PageView(CaptionPrefix);
                    OnWindowLoad();
                    await OnWindowLoadAsync();
                }
                catch (Exception ex)
                {
                    OnError(ex, "Window Initialize");
                }
            });
        }


        private void HandleWindowClosing(Window win, bool hideOnWindowClose)
        {
            win.Closing += async (s, e) =>
            {
                e.Cancel = true;
                if (hideOnWindowClose)
                {
                    AppInsights.Post($"Hiding “{CaptionPrefix}” instead of closing it");
                    win.Hide();
                }
                else
                {
                    AppInsights.Post($"Closing “{CaptionPrefix}”");
                    await ExitCmd.RunAsync();
                }
            };
        }


        private void HandleGlobalErrors()
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
        }
    }
}
