using CommonTools.Lib.ns11.StringTools;
using System;
using System.Drawing;
using System.IO;
using System.Windows;

namespace CommonTools.Lib.fx45.ImagingTools
{
    public static class CreateBitmap
    {
        public static Bitmap FromPrimaryScreen()
        {
            var width  = (int)SystemParameters.PrimaryScreenWidth;
            var height = (int)SystemParameters.PrimaryScreenHeight;
            return FromScreenRegion(0, 0, width, height);
        }


        public static Bitmap FromScreenRegion(int x, int y, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            var gpx = Graphics.FromImage(bmp);
            gpx.CopyFromScreen(x, y, 0, 0, bmp.Size);
            return bmp;
        }


        public static Bitmap FromBase64(string b64EncodedBitmap)
        {
            if (b64EncodedBitmap.IsBlank()) return null;
            var byts = Convert.FromBase64String(b64EncodedBitmap);
            using (var ms = new MemoryStream(byts))
            {
                return new Bitmap(ms);
            }
        }
    }
} 
