using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommonTools.Lib.ns11.StringTools;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public class CurrentExeTools
    {
    }


    public static class CurrentExe
    {
        public static string GetFullPath()
        {
            return Assembly.GetEntryAssembly()?.Location;
        }


        public static string GetDirectory()
        {
            var exe = GetFullPath();
            if (exe.IsBlank()) return string.Empty;
            return Directory.GetParent(exe).FullName;
        }


        public static string GetVersion()
        {
            var exe = GetFullPath();
            if (exe.IsBlank()) return string.Empty;
            return FileVersionInfo.GetVersionInfo(exe).FileVersion;
        }
    }
}
