using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using System.IO;
using System.Windows;

namespace FreshCopy.Server.Lib45.FileWatchers
{
    public class BinaryFileWatcherVM : FileWatcherVMBase
    {
        public BinaryFileWatcherVM(IThrottledFileWatcher throttledFileWatcher, 
                                   CommonLogListVM commonLogListVM) 
            : base(throttledFileWatcher, commonLogListVM)
        {
        }


        protected override void OnFileChanged(string fileKey, string filePath)
        {
            var nme = Path.GetFileName(filePath);
            MessageBox.Show($"“{fileKey}” changed: {nme}");
        }
    }
}
