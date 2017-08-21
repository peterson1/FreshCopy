using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.FileSystemTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Common.API.HubServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    [HubName(VersionKeeperHub.Name)]
    public class VersionKeeperHub1 : Hub, IVersionKeeperServer
    {
        private VersionKeeperSettings _cfg;
        private SharedLogListVM       _logs;


        public VersionKeeperHub1(VersionKeeperSettings versionKeeperSettings,
                                 SharedLogListVM sharedLogListVM)
        {
            _cfg  = versionKeeperSettings;
            _logs = sharedLogListVM;
        }


        public async Task<string> GetLatestB64(string fileKey)
        {
            await Task.Delay(0);

            if (!IsValidFile(fileKey, out string filePath))
                return string.Empty;

            try
            {
                return CompressThenBase64(filePath);
            }
            catch (Exception ex)
            {
                _logs.Add(ex);
                return string.Empty;
            }
        }


        private static string CompressThenBase64(string filePath)
        {
            var compressd = Path.GetTempFileName();
            filePath.LzmaEncodeAs(compressd);
            return compressd.ReadFileAsBase64();
        }


        public async Task<string> GetLatestSHA1(string fileKey)
        {
            await Task.Delay(0);

            if (!IsValidFile(fileKey, out string filePath))
                return string.Empty;

            try
            {
                return filePath.SHA1ForFile();
            }
            catch (Exception ex)
            {
                _logs.Add(ex);
                return string.Empty;
            }
        }


        private bool IsValidFile(string fileKey, out string filePath)
        {
            if (!_cfg.BinaryFiles.TryGetValue(fileKey, out filePath))
            {
                Log($"Unrecognized BinaryFile key: “{fileKey}”");
                return false;
            }

            if (!File.Exists(filePath))
            {
                Log($"Missing binary file: {filePath}");
                return false;
            }

            return true;
        }


        private void Log(string message) => _logs.Add(message);
    }
}
