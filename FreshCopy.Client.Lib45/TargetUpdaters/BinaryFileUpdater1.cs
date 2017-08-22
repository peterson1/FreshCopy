using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.TargetUpdaters;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.TargetUpdaters
{
    public class BinaryFileUpdater1 : IBinaryFileUpdater
    {
        private string               _fileKey;
        private string               _filePath;
        private ILogList             _logs;
        private bool                 _isBusy;
        private IVersionKeeperClient _client;


        public BinaryFileUpdater1(IVersionKeeperClient versionKeeperClient)
        {
            _client = versionKeeperClient;
        }


        public void ApplyChangesIfNeeded(string remoteFileSHA1)
        {
            if (_isBusy)
            {
                Log($"‹{typeof(IBinaryFileUpdater).Name}› cannot process the request to [{nameof(IBinaryFileUpdater.ApplyChangesIfNeeded)}] while a previous request is running.");
                return;
            }
            _isBusy = true;

            Task.Run(async () =>
            {
                try
                {
                    await ApplyChangesIfNeededAsync(remoteFileSHA1);
                }
                catch (Exception ex)
                {
                    _logs.Add(ex);
                }
                _isBusy = false;
            });
        }


        private async Task ApplyChangesIfNeededAsync(string remoteFileSHA1)
        {
            if (SameHashes(remoteFileSHA1)) return;

            await DownloadAndWriteToDisk();

            var newerRemoteHash = await _client.GetLatestSHA1(_fileKey);
            await ApplyChangesIfNeededAsync(newerRemoteHash);

            if (_fileKey == CheckerRelease.FileKey)
                CurrentExe.RelaunchApp();
        }


        private async Task DownloadAndWriteToDisk()
        {
            Log("Downloading latest file from server ...");
            var b64 = await _client.GetLatestB64(_fileKey);
            if (b64.IsBlank())
                Log("Something went wrong!");

            Log("Writing downloaded file to disk ...");
            DecodeB64ToDisk(b64, _filePath);
        }


        protected virtual void DecodeB64ToDisk(string b64, string targetPath)
        {
            var compressd = Path.GetTempFileName();
            b64.WriteBase64ToFile(compressd);
            compressd.LzmaDecodeAs(targetPath);
        }


        private bool SameHashes(string remoteFileSHA1)
        {
            if (!File.Exists(_filePath))
            {
                Log("Local file currently does not exist.");
                return false;
            }
            var localSHA1 = _filePath.SHA1ForFile();
            var isSame    = localSHA1 == remoteFileSHA1;
            Log(isSame ? "Local hash matches that of remote."
                       : "Local hash differs from that of remote.");
            return isSame;
        }


        public void SetTarget(string fileKey, string filePath, ILogList logList)
        {
            _fileKey  = fileKey;
            _filePath = filePath;
            _logs     = logList;
            _client.SetLogger(logList);
        }


        protected void Log(string message) => _logs.Add(message);
    }
}
