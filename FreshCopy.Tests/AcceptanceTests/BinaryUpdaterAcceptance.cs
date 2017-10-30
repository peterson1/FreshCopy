using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.FileFactories;
using FreshCopy.Tests.ProcessStarters;
using FreshCopy.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Batch", "1")]
    [Trait("Update Checker", "Acceptance")]
    public class BinaryUpdaterAcceptance
    {
        [Fact(DisplayName = "Updates Cold Target")]
        public async Task UpdatesColdTarget()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy(".locFile1");
            var server  = FcServer.StartWith(svrFile, 1, out VersionKeeperSettings cfg);
            var client  = await FcClient.StartWith(locFile, cfg);

            FileChange.Trigger(svrFile);
            await Task.Delay(1000 * 2);

            locFile.MustMatchHashOf(svrFile);

            await TmpDir.Cleanup(server, svrFile, client, locFile);
        }
    }
}
