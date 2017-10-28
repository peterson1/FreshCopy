using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.FileFactories;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FreshCopy.Tests.ProcessStarters
{
    class FcClient
    {
        private const string DEBUG_DIR = @"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug";
        private const string EXE_NAME  = "FC.UpdateChecker.exe";


        internal static Process StartWatching(string filePath, VersionKeeperSettings serverCfg)
        {
            var tmpDir = CreateDir.InTemp();
            var tmpExe = Path.Combine(tmpDir, EXE_NAME);
            File.Copy(GetDebugExe(), tmpExe);

            var cfgUri = Path.Combine(tmpDir, UpdateCheckerCfgFile.FILE_NAME);
            var cfgObj = ComposeCfg(filePath, serverCfg);
            JsonFile.Write(cfgObj, cfgUri);
            return Process.Start(tmpExe);
        }


        private static UpdateCheckerSettings ComposeCfg(string filePath, VersionKeeperSettings serverCfg) => new UpdateCheckerSettings
        {
            ServerURL   = serverCfg.ServerURL,
            UserAgent   = "test client",
            SharedKey   = serverCfg.SharedKey,
            UpdateSelf  = false,
            CanExitApp  = true,
            BinaryFiles = new Dictionary<string, string>
            {
                { serverCfg.BinaryFiles.First().Key, filePath },
            }
        };


        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);
    }
}
