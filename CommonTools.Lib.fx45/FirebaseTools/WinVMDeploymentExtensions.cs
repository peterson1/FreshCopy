using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.GoogleTools;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public static class WinVMDeploymentExtensions
    {
        public static async Task SetupAutoUpdate(this MainWindowVmBase vm, string fileKey, string deploymentKey, string instrumentationKey)
        {
            var json  = AESThenHMAC.SimpleDecryptWithPassword(deploymentKey, instrumentationKey);
            var creds = JsonConvert.DeserializeObject<FirebaseCredentials>(json);
            var conn  = new FirebaseConnection(creds);
            var agtUp = new AgentStateUpdater(conn, creds);
            var vChkr = new NewVersionWatcher(conn, agtUp);
            var origE = CurrentExe.GetFullPath();

            if (await vChkr.NewVersionInstalled(fileKey))
            {
                CurrentExe.Shutdown();
                Process.Start(origE);
            }
        }
    }
}
