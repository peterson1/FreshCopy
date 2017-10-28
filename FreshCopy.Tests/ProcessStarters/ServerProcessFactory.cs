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


        internal static Process StartWatching(string filePath, int portOffset)
        {
            var tmpDir = CreateDir.InTemp();
            var tmpExe = Path.Combine(tmpDir, EXE_NAME);
            File.Copy(GetDebugExe(), tmpExe);

            var cfgUri = Path.Combine(tmpDir, VersionKeeperCfgFile.FILE_NAME);
            var cfgObj = ComposeCfg(portOffset);
            //JsonFile.Write(cfg, )
            return null;
        }


        private static VersionKeeperSettings ComposeCfg(int portOffset) => new VersionKeeperSettings
        {
            ServerURL            = "http://localhost:12345",
            SharedKey            = "abc123",
            MasterCopy           = @"c:\path\to\master\copy.exe",
            DisconnectTimeoutHrs = 23,
            BinaryFiles          = new Dictionary<string, string>
            {
                { "small text file", "smallText_src.txt" },
                { "big text file"  , "bigText_src.txt"   },
                { CheckerRelease.FileKey, "path to official checker release" },
            },
            AppendOnlyDBs        = new Dictionary<string, string>
            {
                { "sample LiteDB 1", "sampleLiteDB1.LiteDB3" },
            }
        };


        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);
    }
}
