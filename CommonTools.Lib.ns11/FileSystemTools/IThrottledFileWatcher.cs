namespace CommonTools.Lib.ns11.FileSystemTools
{
    public interface IThrottledFileWatcher : IFileChangeWatcher
    {
        uint IntervalMS { get; set; }
    }
}
