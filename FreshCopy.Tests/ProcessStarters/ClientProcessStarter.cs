using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ThreadTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    class StartClient
    {
        private const string DEBUG_DIR = @"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug";
        private const string EXE_NAME  = "FC.UpdateChecker.exe";


        public static Process WatchFile(string fileKey, out string targPath)
        {
            targPath = Cfg().BinaryFiles[fileKey];
            return Process.Start(GetDebugExe());
        }

        public static Process WatchDB(string fileKey, out string targPath)
        {
            targPath = Cfg().AppendOnlyDBs[fileKey];
            return Process.Start(GetDebugExe());
        }

        public static Process WatchExe(string fileKey, out string targPath)
        {
            targPath = Cfg().Executables[fileKey];
            return Process.Start(GetDebugExe());
        }


        public static Process WatchAnyFile()
            => WatchExe("R2 Uploader", out string path);



        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);

        private static string GetCfgPath()
            => Path.Combine(DEBUG_DIR, UpdateCheckerCfgFile.FILE_NAME);

        private static UpdateCheckerSettings Cfg()
            => JsonFile.Read<UpdateCheckerSettings>(GetCfgPath());
    }


    class EndClient
    {
        public static void Process()
        {
            var nme = Path.GetFileName(StartClient.GetDebugExe());
            KillProcess.ByName(nme, true);
        }
    }
}
