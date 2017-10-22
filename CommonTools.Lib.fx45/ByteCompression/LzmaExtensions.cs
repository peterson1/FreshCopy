using CommonTools.Lib.fx45.FileSystemTools;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System;
using System.IO;
using System.Linq;

namespace CommonTools.Lib.fx45.ByteCompression
{
    public static class LzmaExtensions
    {
        public static void LzmaEncodeAs (this string sourcePath, string targetPath)
        {
            using (var archive = ZipArchive.Create())
            {
                try
                {
                    archive.AddEntry("soloFile", sourcePath);
                }
                catch (IOException)
                {
                    var tmp = sourcePath.CreateTempCopy();
                    archive.AddEntry("soloFile", tmp);
                }
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


        public static string LzmaEncodeThenB64(this string filePath)
        {
            var compressd = Path.GetTempFileName();
            filePath.LzmaEncodeAs(compressd);
            var b64 = compressd.ReadFileAsBase64();
            File.Delete(compressd);
            return b64;
        }


        public static string LzmaDecodeB64ToTemp(this string lzmaB64String, string extension = ".tmp")
        {
            var temp = Path.GetTempFileName() + extension;
            var byts = Convert.FromBase64String(lzmaB64String);
            var lzma = Path.GetTempFileName();
            File.WriteAllBytes(lzma, byts);
            lzma.LzmaDecodeAs(temp);
            File.Delete(lzma);
            return temp;
        }
    }
}
