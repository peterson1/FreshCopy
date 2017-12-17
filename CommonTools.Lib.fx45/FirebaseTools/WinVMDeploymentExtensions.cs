using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.GoogleTools;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public static class WinVMDeploymentExtensions
    {
        private static NewVersionWatcher _vChkr;
        private static string            _fileKey;
        private static bool              _isChecking;

        public static void SetupAutoUpdate(this MainWindowVmBase vm, string fileKey, string deploymentKey, string instrumentationKey)
        {
            _vChkr   = CreateWatcher(deploymentKey, instrumentationKey);
            _fileKey = fileKey;
            vm.OnWindowHidden += Vm_OnWindowHidden;
        }


        private static async void Vm_OnWindowHidden(object sender, EventArgs e)
        {
            if (_isChecking) return;
            _isChecking = true;
            var oldExe = CurrentExe.GetFullPath();
            if (await _vChkr.NewVersionInstalled(_fileKey))
            {
                CurrentExe.Shutdown();
                Process.Start(oldExe);
            }
            _isChecking = false;
        }


        private static NewVersionWatcher CreateWatcher(string deploymentKey, string instrumentationKey)
        {
            var json  = AESThenHMAC.SimpleDecryptWithPassword(deploymentKey, instrumentationKey);
            var creds = JsonConvert.DeserializeObject<FirebaseCredentials>(json);
            var conn  = new FirebaseConnection(creds);
            var agtUp = new AgentStateUpdater(conn, creds);
            return new NewVersionWatcher(conn, agtUp);
        }


    }
}
