using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.InputTools;

namespace CommonTools.Lib.fx45.UserControls.AppUpdateNotifiers
{
    public class AppUpdateNotifierVM : ViewModelBase
    {
        private IThrottledFileWatcher _watchr;

        public AppUpdateNotifierVM(IThrottledFileWatcher throttledFileWatcher)
        {
            _watchr = throttledFileWatcher;
            _watchr.FileChanged += (s, e) 
                => RelaunchNowCmd = R2Command.Relay(CurrentExe.RelaunchApp);
        }


        public IR2Command   RelaunchNowCmd   { get; private set; }


        public void StartChecking()
            => _watchr.StartWatching(CurrentExe.GetFullPath());
    }
}
