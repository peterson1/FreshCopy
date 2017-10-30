using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.FileFactories;
using FreshCopy.Tests.ProcessStarters;
using FreshCopy.Tests.TestTools;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Batch", "1")]
    public class BinaryUpdaterAcceptance
    {
        [Fact(DisplayName = "Updates Cold Target")]
        public async Task UpdatesColdTarget()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy();
            var server  = FcServer.StartWith(svrFile, 1, out VersionKeeperSettings cfg);
            var client  = await FcClient.StartWith(locFile, cfg);

            FileChange.Trigger(svrFile);
            await Task.Delay(1000 * 2);

            locFile.MustMatchHashOf(svrFile);

            await TmpDir.Cleanup(server, svrFile, client, locFile);
        }


        [Fact(DisplayName = "Updates Hot Source")]
        public async Task UpdatesHotSource()
        {
            var svrFile = CreateFile.WithRandomText();
            var locFile = svrFile.MakeTempCopy();
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
