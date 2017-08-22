using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.ProcessStarters;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Self-Update", "Acceptance")]
    public class SelfUpdateFacts
    {
        [Fact(DisplayName = "Updater Updates Itself")]
        public async Task UpdaterUpdatesItself()
        {
            var server = Start.ServerProcess(CheckerRelease.FileKey, out string srcPath);
            await Task.Delay(1000 * 2);

            Start.ClientProcess();
            await Task.Delay(1000 * 2);

            FileChange.Trigger(srcPath);
            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 5);

            var targPath = CHECKER.DEBUG;
            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            server.CloseMainWindow();
            End.ClientProcess();
        }
    }
}
