using System.Threading;
using System.Windows;

namespace CommonTools.Lib.fx45.ThreadTools
{
    public class Alert
    {
        public static void Show(string message)
            => new Thread(new ThreadStart(delegate
            {
                MessageBox.Show(message);
            }
            )).Start();
    }
}
