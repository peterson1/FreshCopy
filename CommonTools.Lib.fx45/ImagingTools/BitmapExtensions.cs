using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace CommonTools.Lib.fx45.ImagingTools
{
    public static class BitmapExtensions
    {
        ////https://stackoverflow.com/a/1069509/3973863
        //public static BitmapImage ToImagePNG(this Bitmap bmp)
        //{
        //    var img = new BitmapImage();
        //    using (var mem = new MemoryStream())
        //    {
        //        bmp.Save(mem, ImageFormat.Png);
        //        mem.Position = 0;
        //        img.BeginInit();
        //        img.CacheOption = BitmapCacheOption.OnLoad;
        //        img.StreamSource = mem;
        //        img.EndInit();
        //    }
        //    return img;
        //}


        //public static string ConvertToBase64(this Bitmap bitmap)
        //{
        //    var imgCnv = new ImageConverter();
        //    var byts = (byte[])imgCnv.ConvertTo(bitmap, typeof(byte[]));
        //    return Convert.ToBase64String(byts);
        //}

        public static string ToTempPNG(this Bitmap bmp)
        {
            var path = Path.GetTempFileName();
            bmp.Save(path, ImageFormat.Png);
            return path;
        }
    }
}
