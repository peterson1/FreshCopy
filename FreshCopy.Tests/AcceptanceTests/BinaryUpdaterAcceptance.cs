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
    [Trait("Update Checker", "Acceptance")]
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


        [Fact(DisplayName = "Updates Running Exe")]
        public async Task UpdatesRunningExe()
        {
            var svrExe = CreateFile.TempCopy("windirstat.exe");
            var locExe = svrExe.MakeTempCopy(".exe");
            var server = FcServer.StartWith(svrExe, 3, out VersionKeeperSettings cfg);
            var client = await FcClient.StartWith(locExe, cfg);
            var locPrc = Process.Start(locExe);

            FileChange.Trigger(svrExe);
            await Task.Delay(1000 * 2);

            locExe.MustMatchHashOf(svrExe);

            locPrc.Kill(); locPrc.Dispose();

            await TmpDir.Cleanup(server, svrExe, client, locExe);
        }


        [Fact(DisplayName = "Updates Self")]
        public async Task UpdatesSelf()
        {
            var svrExe = FcClient.GetDebugExe().MakeTempCopy();
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
