using CommonTools.Lib.fx45.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using FreshCopy.Tests.FileFactories;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    class FcServer
    {
        private const string DEBUG_DIR = @"..\..\..\FreshCopy.VersionKeeper.WPF\bin\Debug";
        private const string EXE_NAME  = "FC.VersionKeeper.exe";


        internal static Process StartWatching(string filePath, int portOffset, out VersionKeeperSettings serverCfg)
        {
            var tmpDir = CreateDir.InTemp();
            var tmpExe = Path.Combine(tmpDir, EXE_NAME);
            File.Copy(GetDebugExe(), tmpExe);

            var cfgUri = Path.Combine(tmpDir, VersionKeeperCfgFile.FILE_NAME);
            serverCfg  = ComposeCfg(filePath, portOffset);
            JsonFile.Write(serverCfg, cfgUri);
            return Process.Start(tmpExe);
        }


        private static VersionKeeperSettings ComposeCfg(string filePath, int portOffset) => new VersionKeeperSettings
        {
            ServerURL   = ComposeServerURL(portOffset),
            SharedKey   = Path.GetRandomFileName(),
            BinaryFiles = new Dictionary<string, string>
            {
                { "binary1", filePath },
            }
        };


        private static string ComposeServerURL(int portOffset)
            => $"http://localhost:{ushort.MaxValue - portOffset}";


        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);
    }
}
