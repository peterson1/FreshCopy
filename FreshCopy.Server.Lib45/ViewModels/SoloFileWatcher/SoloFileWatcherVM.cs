using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.InputTools;
using System;

namespace FreshCopy.Server.Lib45.ViewModels.SoloFileWatcher
{
    public class SoloFileWatcherVM : ViewModelBase
    {
        private IThrottledFileWatcher _watchr;
        private string                _filePath;
        private CommonLogListVM       _log;


        public SoloFileWatcherVM(IThrottledFileWatcher throttledFileWatcher,
                                 CommonLogListVM commonLogListVM)
        {
            _log = commonLogListVM;

            _watchr = throttledFileWatcher;
            _watchr.FileChanged += _watchr_FileChanged;

            StartWatchingCmd = R2Command.Relay(StartWatchingFile);
            StopWatchingCmd  = R2Command.Relay( StopWatchingFile);
        }


        public IR2Command   StartWatchingCmd   { get; }
        public IR2Command   StopWatchingCmd    { get; }


        private void _watchr_FileChanged(object sender, EventArgs e)
        {
            _log.Add($"changed: {_filePath}");
        }


        private void StartWatchingFile()
        {
            _watchr.IntervalMS = 1000 * 5;
            _watchr.StartWatching(_filePath);
            _log.Add($"Started watching {_filePath}");
        }


        private void StopWatchingFile()
        {
            _watchr.StopWatching();
            _log.Add($"Stopped watching {_filePath}");
        }


        public void SetTarget(string filePath)
        {
            _filePath = filePath;
        }
    }
}
