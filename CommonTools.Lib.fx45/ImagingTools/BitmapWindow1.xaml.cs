using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.ns11.StringTools;
using System.IO;
using System.Windows;

namespace CommonTools.Lib.fx45.ImagingTools
{
    public partial class BitmapWindow1 : Window
    {
        public BitmapWindow1(string caption, string uriPath)
        {
            InitializeComponent();
            Title      = caption;
            img.Source = BmpImage.FromFile(uriPath);
        }


        public static void Show(string caption, string pngLzmaB64)
        {
            if (pngLzmaB64.IsBlank()) return;
            var tmp = pngLzmaB64.LzmaDecodeB64ToTemp(".png");
            var win = new BitmapWindow1(caption, tmp);
            win.Show();
            File.Delete(tmp);
        }
    }
}
