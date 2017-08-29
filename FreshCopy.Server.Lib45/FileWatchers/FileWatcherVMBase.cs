using CommonTools.Lib.fx45.InputTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.InputTools;
using System;
using System.IO;

namespace FreshCopy.Server.Lib45.FileWatchers
{
    public abstract class FileWatcherVMBase : ViewModelBase
    {
        private IThrottledFileWatcher _watchr;
        private string                _fileKey;
        private string                _filePath;
        private SharedLogListVM       _log;


        public FileWatcherVMBase(IThrottledFileWatcher throttledFileWatcher,
                                 SharedLogListVM commonLogListVM)
        {
            _log    = commonLogListVM;
            _watchr = throttledFileWatcher;
            _watchr.FileChanged += _watchr_FileChanged;

            StartWatchingCmd = R2Command.Relay(StartWatchingFile);
            StopWatchingCmd  = R2Command.Relay( StopWatchingFile);
        }


        public IR2Command   StartWatchingCmd   { get; }
        public IR2Command   StopWatchingCmd    { get; }


        protected abstract void OnFileChanged(string fileKey, string filePath);


        private void _watchr_FileChanged(object sender, EventArgs e)
        {
            var nme = Path.GetFileName(_filePath);
            _log.Add($"“{_fileKey}” changed: {nme}");
            OnFileChanged(_fileKey, _filePath);
        }


        private void StartWatchingFile()
        {
            _watchr.IntervalMS = 1000;
            try
            {
                _watchr.StartWatching(_filePath);
                _log.Add($"Started watching {_filePath}");
            }
            catch (FileNotFoundException)
            {
                _log.Add($"Missing file: “{_fileKey}” {_filePath}");
            }
        }


        private void StopWatchingFile()
        {
            _watchr.StopWatching();
            _log.Add($"Stopped watching {_filePath}");
        }


        public void SetTargetFile(string fileKey, string filePath)
        {
            _fileKey  = fileKey;
            _filePath = filePath;
        }
    }
}
