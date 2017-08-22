using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreshCopy.Common.API.HubServers
{
    public struct VersionKeeperHub
    {
        public const string Name = "VersionKeeperHub";
    }


    public interface IVersionKeeperServer
    {
        Task<string>        GetContentB64  (string fileKey);
        Task<string>        GetLatestSHA1  (string fileKey);
        Task<List<string>>  GetRecords     (string fileKey, long startId);
        Task<long>          GetMaxId       (string fileKey);
    }
}
