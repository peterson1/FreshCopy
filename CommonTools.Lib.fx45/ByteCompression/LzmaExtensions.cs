using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System.Linq;

namespace CommonTools.Lib.fx45.ByteCompression
{
    public static class LzmaExtensions
    {
        public static void LzmaEncodeAs (this string sourcePath, string targetPath)
        {
            using (var archive = ZipArchive.Create())
            {
                archive.AddEntry("soloFile", sourcePath);
                archive.SaveTo(targetPath, CompressionType.LZMA);
            }
        }


        public static void LzmaDecodeAs(this string sourcePath, string targetPath)
        {
            using (var archive = ZipArchive.Open(sourcePath))
            {
                var entry = archive.Entries.First(_ => !_.IsDirectory);
                entry.WriteToFile(targetPath);
            }
        }
    }
}
