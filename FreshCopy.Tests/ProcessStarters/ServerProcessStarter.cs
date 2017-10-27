using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ThreadTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    class StartServer
    {
        private const string DEBUG_DIR = @"..\..\..\FreshCopy.VersionKeeper.WPF\bin\Debug";
        private const string EXE_NAME  = "FC.VersionKeeper.exe";


        public static Process WatchFile(string fileKey, out string srcPath)
        {
            var cfg = JsonFile.Read<VersionKeeperSettings>(GetCfgPath());
            srcPath = cfg.BinaryFiles[fileKey];
            return Process.Start(GetDebugExe());
        }


        public static Process WatchDB(string fileKey, out string srcPath)
        {
            var cfg = JsonFile.Read<VersionKeeperSettings>(GetCfgPath());
            srcPath = cfg.AppendOnlyDBs[fileKey];
            return Process.Start(GetDebugExe());
        }

        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);

        private static string GetCfgPath()
            => Path.Combine(DEBUG_DIR, VersionKeeperCfgFile.FILE_NAME);
    }


    class EndServer
    {
        public static void Process()
        {
            var nme = Path.GetFileName(StartServer.GetDebugExe());
            KillProcess.ByName(nme, true);
        }
    }
}
