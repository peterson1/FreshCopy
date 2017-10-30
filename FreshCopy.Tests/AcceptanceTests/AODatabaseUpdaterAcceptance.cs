using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.CustomAssertions;
using FreshCopy.Tests.ProcessStarters;
using FreshCopy.Tests.SampleDatabases;
using FreshCopy.Tests.TestTools;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Update Checker", "Acceptance")]
    public class AODatabaseUpdaterAcceptance
    {
        [Fact(DisplayName = "Updates Local DB")]
        public async Task UpdatesLocalDB()
        {
            var svrDb = SampleDB1.CreateInTemp();
            var locDb = svrDb.MakeTempCopy(".LiteDB");
            var servr = FcServer.StartWith(svrDb, 5, out VersionKeeperSettings cfg);
            var chekr = await FcClient.StartWith(locDb, cfg);

            svrDb.AddRecords(1);
            await Task.Delay(1000 * 2);

            locDb.MustMatchMaxIdOf(svrDb);

            await TmpDir.Cleanup(servr, svrDb, chekr, locDb);
        }
    }
}
