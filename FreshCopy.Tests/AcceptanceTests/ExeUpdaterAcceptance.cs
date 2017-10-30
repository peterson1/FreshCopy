using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.FileFactories;
using FreshCopy.Tests.ProcessStarters;
using FreshCopy.Tests.TestTools;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Batch", "3")]
    [Trait("Update Checker", "Acceptance")]
    public class ExeUpdaterAcceptance
    {
        [Fact(DisplayName = "Updates Running Exe")]
        public async Task UpdatesRunningExe()
        {
            var svrExe = CreateFile.TempCopy("windirstat.exe");
            var locExe = svrExe.MakeTempCopy("locWds.exe");
            var server = FcServer.StartWith(svrExe, 3, out VersionKeeperSettings cfg);
            var client = await FcClient.StartWith(locExe, cfg);
            var locPrc = Process.Start(locExe);

            FileChange.Trigger(svrExe);
            await Task.Delay(1000 * 2);

            locExe.MustMatchHashOf(svrExe);

            locPrc.Kill(); locPrc.Dispose();

            await TmpDir.Cleanup(server, svrExe, client, locExe);
        }
    }
}
