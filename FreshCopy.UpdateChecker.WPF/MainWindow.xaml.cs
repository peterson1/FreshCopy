﻿using CommonTools.Lib.fx45.WindowTools;
using System.Windows;

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
            this.ToggleVisibility();
        }
    }
}
