using System.Diagnostics;

namespace CommonTools.Lib.fx45.ThreadTools
{
    public class KillProcess
    {
        public static void ByName(string processName)
        {
            var cmd = $"taskkill /f /im {processName}";
            Process.Start("cmd.exe", $"/C {cmd}");
        }
    }
}
