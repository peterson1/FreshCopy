using CommonTools.Lib.fx45.FileSystemTools;
using FluentAssertions;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.AcceptanceTests
{
    [Trait("Binary Files", "Acceptance")]
    public class BinaryFileFacts
    {
        [Fact(DisplayName = "Updates Hot File")]
        public async Task UpdatesHotFile()
        {
            var server = StartServerProcess("R2 Uploader", out string srcPath);
            await Task.Delay(1000 * 2);

            var client = StartClientProcess("R2 Uploader", out string targPath);
            await Task.Delay(1000 * 2);

            var hotExe = Process.Start(targPath);
            await Task.Delay(1000 * 1);

            TriggerFileChange(srcPath);
            var srcHash = srcPath.SHA1ForFile();
            await Task.Delay(1000 * 5);

            var targHash = targPath.SHA1ForFile();
            targHash.Should().Be(srcHash);

            server.CloseMainWindow();
            client.CloseMainWindow();
            hotExe.CloseMainWindow();
        }


        [Fact(DisplayName = "Updates Cold File")]
        public async Task Updatesthetarget()
        {
            var server = StartServerProcess("small text file", out string srcPath);
            await Task.Delay(1000 * 2);

            var client = StartClientProcess("small text file", out string targPath);
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
            using (var fileStream = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(DateTime.Now.ToLongTimeString());
            }
        }


        private Process StartClientProcess(string fileKey, out string targPath)
        {
            var nme  = UpdateCheckerCfgFile.FILE_NAME;
            var cfg  = JsonFile.Read<UpdateCheckerSettings>(nme);
            targPath = cfg.BinaryFiles[fileKey];
            return Process.Start(@"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug\FC.UpdateChecker.exe");
        }


        private Process StartServerProcess(string fileKey, out string srcPath)
        {
            var nme = VersionKeeperCfgFile.FILE_NAME;
            var cfg = JsonFile.Read<VersionKeeperSettings>(nme);
            srcPath = cfg.BinaryFiles[fileKey];
            return Process.Start(@"..\..\..\FreshCopy.VersionKeeper.WPF\bin\Debug\FC.VersionKeeper.exe");
        }
    }
}
