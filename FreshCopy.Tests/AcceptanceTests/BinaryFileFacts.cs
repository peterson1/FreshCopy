using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.ProcessStarters;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Binary Files", "Acceptance")]
    public class BinaryFileFacts
    {
        [Fact(DisplayName = "Updates Hot File")]
        public async Task UpdatesHotFile()
        {
            var server = StartServer.WatchFile("R2 Uploader", out string srcPath);
            await Task.Delay(1000 * 2);

            var client = StartClient.WatchFile("R2 Uploader", out string targPath);
            await Task.Delay(1000 * 2);

            var hotExe = Process.Start(targPath);
            await Task.Delay(1000 * 1);

            FileChange.Trigger(srcPath);
            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 5);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            server.CloseMainWindow();
            client.CloseMainWindow();
            hotExe.CloseMainWindow();
        }


        [Fact(DisplayName = "Updates Cold File")]
        public async Task Updatesthetarget()
        {
            var server = StartServer.WatchFile("small text file", out string srcPath);
            await Task.Delay(1000 * 2);

            var client = StartClient.WatchFile("small text file", out string targPath);
            await Task.Delay(1000 * 2);

            FileChange.Trigger(srcPath);
            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 2);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            server.CloseMainWindow();
            client.CloseMainWindow();
        }




    }
}
