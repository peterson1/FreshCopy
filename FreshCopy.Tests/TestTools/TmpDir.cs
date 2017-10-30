using CommonTools.Lib.fx45.FileSystemTools;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace FreshCopy.Tests.TestTools
{
    public class TmpDir
    {
        public static async Task Cleanup(Process serverProc, string serverFile, Process clientProc, string clientFile)
        {
            await Task.WhenAll(DeleteParentDir(serverProc),
                               DeleteParentDir(clientProc));
            serverFile.DeleteIfFound();
            clientFile.DeleteIfFound();
        }


        private static async Task DeleteParentDir(Process proc)
        {
            var exe = proc.MainModule.FileName;
            var dir = Path.GetDirectoryName(exe);
            proc.Kill();
            proc.Dispose();
            await Task.Delay(1000);
            File.Delete(exe);
            Directory.Delete(dir, true);
        }
    }
}
