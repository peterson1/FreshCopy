using System;
using System.IO;

namespace FreshCopy.Tests.ChangeTriggers
{
    class FileChange
    {
        public static void Trigger(string filepath)
        {
            using (var fileStream = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(DateTime.Now.ToLongTimeString());
            }
        }
    }
}
