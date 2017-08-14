using CommonTools.Lib.ns11.FileSystemTools;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.IO;

namespace CommonTools.Lib.fx45.FileSystemTools
{
    public class FileChangeWatcher1 : IFileChangeWatcher, IDisposable
    {
        private      EventHandler _fileChanged;
        public event EventHandler  FileChanged
        {
            add    { _fileChanged -= value; _fileChanged += value; }
            remove { _fileChanged -= value; }
        }

        private FileSystemWatcher _fsWatchr;



        public void StartWatching(string filepath)
        {
            if (_fsWatchr != null) return;

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"File not found:{L.f}{filepath}");

            var dir = Path.GetDirectoryName(filepath);
            var nme = Path.GetFileName(filepath);

            _fsWatchr                     = new FileSystemWatcher(dir, nme);
            _fsWatchr.NotifyFilter        = NotifyFilters.LastWrite;
            _fsWatchr.Changed            += new FileSystemEventHandler(OnLdbChanged);
            _fsWatchr.EnableRaisingEvents = true;
        }


        private void OnLdbChanged(object sender, FileSystemEventArgs e)
        {
            RaiseFileChanged();
        }


        protected virtual void RaiseFileChanged()
        {
            _fileChanged?.Invoke(this, EventArgs.Empty);
        }


        public void StopWatching()
        {
            if (_fsWatchr == null) return;
            _fsWatchr.EnableRaisingEvents = false;
            _fsWatchr = null;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopWatching();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
