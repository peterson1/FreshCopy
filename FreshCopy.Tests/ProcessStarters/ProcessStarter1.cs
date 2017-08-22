using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ThreadTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    public struct CHECKER
    {
        public const string DEBUG = @"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug\FC.UpdateChecker.exe";
    }

    public struct KEEPER
    {
        public const string DEBUG = @"..\..\..\FreshCopy.VersionKeeper.WPF\bin\Debug\FC.VersionKeeper.exe";
    }


    class Start
    {
        public static Process ClientProcess(string fileKey, out string targPath)
        {
            var nme  = UpdateCheckerCfgFile.FILE_NAME;
            var cfg  = JsonFile.Read<UpdateCheckerSettings>(nme);
            targPath = cfg.BinaryFiles[fileKey];
            return Process.Start(CHECKER.DEBUG);
        }


        public static Process ClientProcess()
            => ClientProcess("R2 Uploader", out string path);


        public static Process ServerProcess(string fileKey, out string srcPath)
        {
            var nme = VersionKeeperCfgFile.FILE_NAME;
            var cfg = JsonFile.Read<VersionKeeperSettings>(nme);
            srcPath = cfg.BinaryFiles[fileKey];
            return Process.Start(KEEPER.DEBUG);
        }
    }


    class End
    {
        public static void ClientProcess()
        {
            var nme = Path.GetFileName(CHECKER.DEBUG);
            KillProcess.ByName(nme);
        }
    }
}
