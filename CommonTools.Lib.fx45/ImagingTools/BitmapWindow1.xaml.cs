using System.Drawing;
using System.Windows;

namespace CommonTools.Lib.fx45.ImagingTools
{
    public partial class BitmapWindow1 : Window
    {
        public BitmapWindow1()
        {
            InitializeComponent();
        }


        public static void Show(string caption, Bitmap bitmap)
        {
            var win = new BitmapWindow1();
            win.Title = caption;
            win.img.Source = bitmap?.ConvertToImage();
            win.Show();
        }
    }
}
