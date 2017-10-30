using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.FileFactories;
using FreshCopy.Tests.ProcessStarters;
using FreshCopy.Tests.TestTools;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Batch", "2")]
    [Trait("Update Checker", "Acceptance")]
    public class BinaryUpdaterAcceptance2
    {
        [Fact(DisplayName = "Updates Hot Source")]
        public async Task UpdatesHotSource()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy(".locFile2");
            var server  = FcServer.StartWith(svrFile, 2, out VersionKeeperSettings cfg);

            FileChange.Trigger(svrFile);
            var fileRef = new FileStream(svrFile, FileMode.Open, FileAccess.ReadWrite);

            var client  = await FcClient.StartWith(locFile, cfg);
            await Task.Delay(1000 * 2);
            fileRef.Dispose();

            locFile.MustMatchHashOf(svrFile);

            await TmpDir.Cleanup(server, svrFile, client, locFile);
        }
    }
}
