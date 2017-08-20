using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Happy Path", "Acceptance")]
    public class SimpleHappyPath
    {
        [Fact(DisplayName = "Updates the target")]
        public async Task Updatesthetarget()
        {
            var server = StartServerProcess(out string srcPath);
            var client = StartClientProcess(out string targPath);

            await Task.Delay(1000 * 2);

            TriggerFileChange(srcPath);
            var srcHash = srcPath.SHA1ForFile();

            await Task.Delay(1000 * 2);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            server.CloseMainWindow();
            client.CloseMainWindow();
        }


        private void TriggerFileChange(string filepath)
        {
            var content = DateTime.Now.ToLongTimeString();
            File.WriteAllText(filepath, content);
        }


        private Process StartClientProcess(out string targPath)
        {
            var nme = UpdateCheckerCfgFile.FILE_NAME;
            //JsonFile.Write(UpdateCheckerSettings.CreateDefault(), nme);
            var cfg = JsonFile.Read<UpdateCheckerSettings>(nme);
            targPath = cfg.BinaryFiles.First().Value;
            return Process.Start(@"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug\FC.UpdateChecker.exe");
        }


        private Process StartServerProcess(out string srcPath)
        {
            var nme = VersionKeeperCfgFile.FILE_NAME;
            //JsonFile.Write(VersionKeeperSettings.CreateDefault(), nme);
            var cfg = JsonFile.Read<VersionKeeperSettings>(nme);
            srcPath = cfg.BinaryFiles.First().Value;
            return Process.Start(@"..\..\..\FreshCopy.VersionKeeper.WPF\bin\Debug\FC.VersionKeeper.exe");
        }
    }
}
