using CommonTools.Lib.fx45.WindowTools;
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

        private void TaskbarIcon_TrayLeftMouseUp(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
                this.ToggleVisibility();
        }
    }
}
