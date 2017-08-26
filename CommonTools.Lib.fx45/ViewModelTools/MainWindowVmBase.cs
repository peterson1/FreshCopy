﻿using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.InputTools;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CommonTools.Lib.fx45.ViewModelTools
{
    public abstract class MainWindowVmBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        protected string _exeVer;


        public MainWindowVmBase()
        {
            _exeVer = CurrentExe.GetVersion();
            ExitCmd = R2Command.Async(ExitApp);
            AppendToCaption("...");
        }

        protected abstract string   CaptionPrefix   { get; }

        public string       Caption           { get; protected set; }
        public int          SelectedTabIndex  { get; set; }
        public IR2Command   ExitCmd           { get; }
        public bool         IsBusy            { get; private set; }
        public string       BusyText          { get; private set; }


        public void HandleWindowEvents(Window win)
        {
            win.Closing += async (s, e) =>
            {
                e.Cancel = true;
                var vm = win.DataContext as MainWindowVmBase;
                await vm.ExitCmd.RunAsync();
            };

            win.DataContext = this;
            Task.Run(async () =>
            {
                try
                {
                    await OnWindowLoad();
                }
                catch (Exception ex)
                {
                    OnError(ex, "Window Initialize");
                }
            });
        }



        protected void StartBeingBusy(string message)
        {
            IsBusy = true;
            BusyText = message;
        }

        protected void StopBeingBusy() => IsBusy = false;





        private async Task ExitApp()
        {
            await OnWindowClose();
            Application.Current.Shutdown();
        }


        protected virtual async Task OnWindowLoad()
        {
            await Task.Delay(1);
        }


        protected virtual async Task OnWindowClose()
        {
            await Task.Delay(1);
        }


        protected virtual void OnError(Exception ex, string taskDescription = null)
        {
            var caption = taskDescription.IsBlank()
                        ? ex.Message
                        : $"Error on “{taskDescription}”";

            new Thread(new ThreadStart(delegate
            {
                MessageBox.Show(ex.Info(true, true), $"   {caption}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            )).Start();
        }


        protected virtual void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {text}";
    }
}
