using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CommonTools.Lib.fx45.UIExtensions
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


        public static void MakeDraggable(this Window win)
        {
            win.MouseDown += (s, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                    win.DragMove();
            };
        }


        public static async void ShowTemporarilyOnTop(this Window win, int delayMS = 500)
        {
            win.Show();
            win.Activate();
            win.Topmost = true;
            await Task.Delay(delayMS);
            win.Topmost = false;
            win.Activate();
        }
    }
}
