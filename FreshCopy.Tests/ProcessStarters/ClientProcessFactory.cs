using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Tests.FileFactories;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace FreshCopy.Tests.ProcessStarters
{
    class FcClient
    {
        private const string DEBUG_DIR = @"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug";
        private const string EXE_NAME  = "FC.UpdateChecker.exe";


        internal static async Task<Process> StartWith(string filePath, VersionKeeperSettings serverCfg, bool updateSelf = false)
        {
            var tmpDir = CreateDir.InTemp();
            var tmpExe = Path.Combine(tmpDir, EXE_NAME);
            File.Copy(GetDebugExe(), tmpExe);

            var cfgUri = Path.Combine(tmpDir, UpdateCheckerCfgFile.FILE_NAME);
            var cfgObj = ComposeCfg(filePath, serverCfg, updateSelf);
            JsonFile.Write(cfgObj, cfgUri);

            await Task.Delay(1000 * 2);
            var proc = Process.Start(tmpExe);

            await Task.Delay(1000 * 4);
            return proc;
        }


        private static UpdateCheckerSettings ComposeCfg(
            string filePath, VersionKeeperSettings serverCfg, bool updateSelf)
        {
            var cfg = new UpdateCheckerSettings
            {
                ServerURL   = serverCfg.ServerURL,
                UserAgent   = "test client",
                SharedKey   = serverCfg.SharedKey,
                UpdateSelf  = updateSelf,
                CanExitApp  = true,
            };
            if (filePath.IsBlank()) return cfg;
            var dict = ComposeDict(filePath, serverCfg);

            if (filePath.EndsWith(".exe"))
                cfg.Executables = dict;
            else
                cfg.BinaryFiles = dict;

            return cfg;
        }

        private static Dictionary<string, string> ComposeDict(string filePath, VersionKeeperSettings serverCfg) => new Dictionary<string, string>
        {
            { serverCfg.BinaryFiles.First().Key, filePath }
        };


        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);


        public static Process FindRunningProcess()
        {
            var nme = Path.GetFileNameWithoutExtension(GetDebugExe());
            return Process.GetProcessesByName(nme).FirstOrDefault();
        }
    }
}
