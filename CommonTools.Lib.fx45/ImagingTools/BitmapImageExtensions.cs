using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CommonTools.Lib.fx45.ImagingTools
{
    //https://social.msdn.microsoft.com/Forums/vstudio/en-US/8327dd31-2db1-4daa-a81c-aff60b63fee6/converting-an-imagebitmapimage-object-into-byte-array-and-vice-versa?forum=wpf
    public static class BitmapImageExtensions
    {
        public static byte[] ToBytes(this BitmapImage imageSource)
        {
            Stream stream = imageSource.StreamSource;
            byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }
            return buffer;
        }
    }

    public class BmpImage
    {
        //https://stackoverflow.com/a/1763678/3973863
        public static BitmapImage FromFile(string filePath)
        {
            BitmapImage image = new BitmapImage();
            using (FileStream stream = File.OpenRead(filePath))
            {
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit(); // load the image from the stream
            } // close the stream
            return image;
        }
    }
}
