using FreshCopy.Client.Lib45.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace FreshCopy.UpdateChecker.WPF
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
            var vm = DataContext as MainCheckerWindowVM;
            await vm.ExitCmd.RunAsync();
        }
    }
}
