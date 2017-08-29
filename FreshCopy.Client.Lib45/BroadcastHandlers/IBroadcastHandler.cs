using CommonTools.Lib.fx45.LoggingTools;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public interface IBroadcastHandler
    {
        string             FileKey  { get; }
        ContextLogListVM   Logs     { get; }

        void SetTargetFile(string fileKey, string filePath);
    }
}
