using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ThreadTools;
using FreshCopy.Client.Lib45.Configuration;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Server.Lib45.Configuration;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    public struct KEEPER
    {
        public const string DEBUG = @"..\..\..\FreshCopy.VersionKeeper.WPF\bin\Debug\FC.VersionKeeper.exe";
    }


    class StartServer
    {
        public static Process WatchFile(string fileKey, out string srcPath)
        {
            var nme = VersionKeeperCfgFile.FILE_NAME;
            var cfg = JsonFile.Read<VersionKeeperSettings>(nme);
            srcPath = cfg.BinaryFiles[fileKey];
            return Process.Start(KEEPER.DEBUG);
        }


        public static Process WatchDB(string fileKey, out string srcPath)
        {
            var nme = VersionKeeperCfgFile.FILE_NAME;
            var cfg = JsonFile.Read<VersionKeeperSettings>(nme);
            srcPath = cfg.AppendOnlyDBs[fileKey];
            return Process.Start(KEEPER.DEBUG);
        }
    }
}
