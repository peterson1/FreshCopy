using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.FileFactories;
using FreshCopy.Tests.ProcessStarters;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Binary Files", "Acceptance")]
    public class BinaryFileFacts
    {
        [Fact(DisplayName = "Updates Hot Target")]
        public async Task UpdatesHotTarget()
        {
            await Task.Delay(1000 * 2);

            StartServer.WatchFile("R2 Uploader", out string srcPath);
            await Task.Delay(1000 * 2);

            StartClient.WatchExe("R2 Uploader", out string targPath);
            await Task.Delay(1000 * 10);

            var hotExe = Process.Start(targPath);
            await Task.Delay(1000 * 2);

            FileChange.Trigger(srcPath);
            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 10);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            EndClient.Process();
            EndServer.Process();
            //hotExe.CloseMainWindow();
            hotExe.Kill();
        }


        [Fact(DisplayName = "Updates Hot Source")]
        public async Task UpdatesHotSource()
        {
            StartServer.WatchFile("small text file", out string srcPath);
            await Task.Delay(1000 * 2);

            StartClient.WatchFile("small text file", out string targPath);
            await Task.Delay(1000 * 10);

            FileChange.Trigger(srcPath);

            using (var fileStream = new FileStream(srcPath, FileMode.Append, FileAccess.Write))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(DateTime.Now.ToLongTimeString());
                await Task.Delay(1000 * 10);
            }


            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 2);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            EndClient.Process();
            EndServer.Process();
        }


        [Fact(DisplayName = "Updates Hot Source 2")]
        public async Task UpdatesHotSource2()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy();
            var server  = FcServer.StartWatching(svrFile, 1, out VersionKeeperSettings cfg);

            FileChange.Trigger(svrFile);
            var stream = new FileStream(svrFile, FileMode.Open, FileAccess.Read, FileShare.None);

            var client  = await FcClient.StartWatching(locFile, cfg);
            await Task.Delay(1000 * 5);
            stream.Dispose();
            await Task.Delay(1000 * 1);

            locFile.MustMatchHashOf(svrFile);

            await Cleanup(server, svrFile, client, locFile);
        }


        [Fact(DisplayName = "Updates Cold Target")]
        public async Task UpdatesColdTarget()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy();
            var server  = FcServer.StartWatching(svrFile, 1, out VersionKeeperSettings cfg);
            var client  = await FcClient.StartWatching(locFile, cfg);

            FileChange.Trigger(svrFile);
            await Task.Delay(1000 * 2);

            locFile.MustMatchHashOf(svrFile);

            await Cleanup(server, svrFile, client, locFile);
        }


        private async Task Cleanup(Process serverProc, string serverFile, Process clientProc, string clientFile)
        {
            await Task.WhenAll(DeleteParentDir(serverProc),
                               DeleteParentDir(clientProc));
            File.Delete(serverFile);
            File.Delete(clientFile);
        }


        private async Task DeleteParentDir(Process proc)
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
