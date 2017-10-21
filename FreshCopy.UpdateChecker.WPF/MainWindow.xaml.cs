using CommonTools.Lib.fx45.WindowTools;
using FreshCopy.Client.Lib45.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace FreshCopy.UpdateChecker.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private MainCheckerWindowVM VM => DataContext as MainCheckerWindowVM;


        private void TaskbarIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
                this.ToggleVisibility();
        }


        private void TaskbarIcon_TrayRightMouseUp(object sender, RoutedEventArgs e)
        {
            VM.SetContextMenu(ctxMenu);
        }
    }
}
