using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.ChangeTriggers;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.ProcessStarters;
using FreshCopy.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Batch", "4")]
    [Trait("Update Checker", "Acceptance")]
    public class SelfUpdaterAcceptance
    {
        [Fact(DisplayName = "Updates Self")]
        public async Task UpdatesSelf()
        {
            var svrExe = FcClient.GetDebugExe().MakeTempCopy(".dbgSvrExe");
            var server = FcServer.StartWith(svrExe, 4,
                            out VersionKeeperSettings cfg,
                            CheckerRelease.FileKey);
            var client = await FcClient.StartWith("", cfg, true);
            var updatr = client.MainModule.FileName;

            FileChange.Trigger(svrExe);
            await Task.Delay(1000 * 4);

            updatr.MustMatchHashOf(svrExe);

            var newProc = FcClient.FindRunningProcess();
            await TmpDir.Cleanup(server, svrExe, newProc, updatr);
        }
    }
}
