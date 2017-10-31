using CommonTools.Lib.fx45.UIExtensions;
using System.Windows;

namespace FreshCopy.ServerControl.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TaskbarIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            this.ToggleVisibility();
        }
    }
}
