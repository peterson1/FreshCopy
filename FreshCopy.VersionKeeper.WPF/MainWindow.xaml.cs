using FreshCopy.Server.Lib45.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace FreshCopy.VersionKeeper.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        protected override async void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            var vm = DataContext as MainVersionKeeperWindowVM;
            await vm.ExitCmd.RunAsync();
        }
    }
}
