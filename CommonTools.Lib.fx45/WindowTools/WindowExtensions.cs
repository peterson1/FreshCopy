using System.Windows;

namespace CommonTools.Lib.fx45.WindowTools
{
    public static class WindowExtensions
    {
        public static void ToggleVisibility(this Window win)
        {
            if (win.Visibility == Visibility.Visible)
                win.Hide();
            else
            {
                win.Show();
                win.Activate();
            }
        }
    }
}
