using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.ns11.SignalRClients;
using System.Windows;
using System.Windows.Controls;

namespace FreshCopy.ServerControl.WPF.CurrentClientele
{
    public partial class SubTableByUserAgent : UserControl
    {
        public SubTableByUserAgent()
        {
            InitializeComponent();
        }

        private void screenShow_Click(object sender, RoutedEventArgs e)
        {
            var btn  = sender as Button;
            var sess = btn.CommandParameter as HubClientSession;
            var b64 = sess?.CurrentState?.ScreenshotB64;
            if (b64 == null) return;
            //Alert.Show($"b64.Length: {b64.Length}");
            var bmp = CreateBitmap.FromBase64(b64);
            var cap = $"[timestamp should be here]  {sess.UserAgent}";
            BitmapWindow1.Show(cap, bmp);
        }
    }
}
