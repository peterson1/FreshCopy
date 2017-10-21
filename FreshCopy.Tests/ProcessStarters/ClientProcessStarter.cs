using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ThreadTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    public struct CHECKER
    {
        public const string DEBUG = @"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug\FC.UpdateChecker.exe";
    }

    class StartClient
    {
        public static Process WatchFile(string fileKey, out string targPath)
        {
            var nme = UpdateCheckerCfgFile.FILE_NAME;
            var cfg = JsonFile.Read<UpdateCheckerSettings>(nme);
            targPath = cfg.BinaryFiles[fileKey];
            return Process.Start(CHECKER.DEBUG);
        }


        public static Process WatchDB(string fileKey, out string targPath)
        {
            var nme = UpdateCheckerCfgFile.FILE_NAME;
            var cfg = JsonFile.Read<UpdateCheckerSettings>(nme);
            targPath = cfg.AppendOnlyDBs[fileKey];
            return Process.Start(CHECKER.DEBUG);
        }


        public static Process WatchFile()
            => WatchFile("R2 Uploader", out string path);


    }


    class EndClient
    {
        public static void Process()
        {
            var nme = Path.GetFileName(CHECKER.DEBUG);
            KillProcess.ByName(nme, true);
        }
    }
}
