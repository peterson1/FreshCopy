using CommonTools.Lib.fx45.LiteDbTools;
using FluentAssertions;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.ProcessStarters;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Append-Only DBs", "Acceptance")]
    public class AppendOnlyDbFacts
    {
        [Fact(DisplayName = "Syncs DB records")]
        public async Task SyncsDBrecords()
        {
            var server = StartServer.WatchDB("SampleRecord DB", out string srcPath);
            await Task.Delay(1000 * 2);

            var client = StartClient.WatchDB("SampleRecord DB", out string targPath);
            await Task.Delay(1000 * 2);

            DbChange.Trigger(srcPath);
            var srcId = AnyLiteDB.GetMaxId(srcPath);
            await Task.Delay(1000 * 5);

            var targId = AnyLiteDB.GetMaxId(targPath);
            targId.Should().Be(srcId);

            server.CloseMainWindow();
            client.CloseMainWindow();
        }
    }
}
