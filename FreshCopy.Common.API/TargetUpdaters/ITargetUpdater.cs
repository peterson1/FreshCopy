using CommonTools.Lib.ns11.LoggingTools;
using FreshCopy.Common.API.ChangeDescriptions;
using System.Threading.Tasks;

namespace FreshCopy.Common.API.TargetUpdaters
{
    public interface ITargetUpdater<T>
        where T : ITargetChangeInfo
    {
        void   SetTarget          (string fileKey, string filePath, ILogList logList);
        Task   RunInitialCheck    ();
        void   OnChangeReceived   (T change);
    }
}
