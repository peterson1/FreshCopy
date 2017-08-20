using System.Threading.Tasks;

namespace FreshCopy.Common.API.HubServers
{
    public struct VersionKeeperHub
    {
        public const string Name = "VersionKeeperHub";
    }


    public interface IVersionKeeperServer
    {
        Task<string>  GetLatestB64  (string fileKey);
        Task<string>  GetLatestSHA1 (string fileKey);
    }
}
