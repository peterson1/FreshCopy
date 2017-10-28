using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
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


        [Fact(DisplayName = "Updates Cold Target")]
        public async Task UpdatesColdTarget()
        {
            await Task.Delay(1000 * 2);

            StartServer.WatchFile("small text file", out string srcPath);
            await Task.Delay(1000 * 2);

            StartClient.WatchFile("small text file", out string targPath);
            await Task.Delay(1000 * 10);

            FileChange.Trigger(srcPath);
            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 2);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            EndClient.Process();
            EndServer.Process();
        }


        [Fact(DisplayName = "Updates Cold Target 2")]
        public async Task UpdatesColdTarget2()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy();

            var server  = FcServer.StartWatching(svrFile, 1);
            var client  = FcClient.StartWatching(locFile, 1);

            FileChange.Trigger(svrFile);
            await Task.Delay(1000 * 2);

            locFile.MustMatchHashOf(svrFile);

            server.Kill(); client.Kill();
        }
    }
}
