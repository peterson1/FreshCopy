using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.StringTools;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public class CurrentExeTools
    {
    }


    public static class CurrentExe
    {
        public static string GetFullPath()
            => Assembly.GetEntryAssembly()?.Location;


        public static string GetShortName()
            => Path.GetFileNameWithoutExtension(GetFullPath());


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
            //return FileVersionInfo.GetVersionInfo(exe).FileVersion;
            return exe.GetVersion();
        }


        public static string GetShortVersion()
        {
            var ver = CurrentExe.GetVersion();
            if (ver.IsBlank()) return "";
            var ss = ver.Split('.');
            if (ss.Length != 4) return ver;
            return $"{ss[2]}.{int.Parse(ss[3])}";
        }


        public static void Shutdown()
            => UIThread.Run(() 
                => Application.Current.Shutdown());


        public static void RelaunchApp()
        {
            Process.Start(GetFullPath());
            CurrentExe.Shutdown();
        }
    }
}
