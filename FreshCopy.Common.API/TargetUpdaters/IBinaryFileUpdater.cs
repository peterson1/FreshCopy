using CommonTools.Lib.ns11.LoggingTools;

namespace FreshCopy.Common.API.TargetUpdaters
{
    public interface IBinaryFileUpdater
    {
        void  SetTarget            (string fileKey, string filePath, ILogList logList);
        void  ApplyChangesIfNeeded (string remoteFileSHA1);
    }
}
