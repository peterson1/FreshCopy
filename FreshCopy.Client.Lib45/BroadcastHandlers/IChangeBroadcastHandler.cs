using CommonTools.Lib.fx45.LoggingTools;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public interface IChangeBroadcastHandler
    {
        string             FileKey  { get; }
        ContextLogListVM   Logs     { get; }

        Task CheckThenSetHandler();
        void SetTargetFile(string fileKey, string filePath);
    }
}
