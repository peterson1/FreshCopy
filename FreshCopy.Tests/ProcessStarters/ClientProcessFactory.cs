using FreshCopy.Common.API.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace FreshCopy.Tests.ProcessStarters
{
    class FcClient
    {
        private const string DEBUG_DIR = @"..\..\..\FreshCopy.UpdateChecker.WPF\bin\Debug";
        private const string EXE_NAME  = "FC.UpdateChecker.exe";


        internal static Process StartWatching(string filePath, VersionKeeperSettings serverCfg)
        {
            throw new NotImplementedException();
        }


        public static string GetDebugExe()
            => Path.Combine(DEBUG_DIR, EXE_NAME);
    }
}
