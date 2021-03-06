﻿using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.StringTools;
using FreshCopy.Client.Lib45.ViewModels;
using FreshCopy.Common.API.ChangeDescriptions;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Common.API.HubClients;
using FreshCopy.Common.API.TargetUpdaters;
using System.IO;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.TargetUpdaters
{
    public class BinaryFileUpdater1 : TargetUpdaterBase<BinaryFileChangeInfo>, IBinaryFileUpdater
    {
        private IMessageBroadcastClient _listnr;
        private TrayContextMenuItems    _trayMnu;


        public BinaryFileUpdater1(IVersionKeeperClient versionKeeperClient,
                                  IMessageBroadcastClient messageBroadcastListener,
                                  TrayContextMenuItems trayContextMenuItems) 
            : base(versionKeeperClient)
        {
            _listnr  = messageBroadcastListener;
            _trayMnu = trayContextMenuItems;
        }


        public async Task RunInitialCheck()
        {
            var newerRemoteHash = await _client.GetLatestSHA1(_fileKey);
            if (newerRemoteHash.IsBlank())
            {
                Log($"GetLatestSHA1('{_fileKey}') returned BLANK");
                return;
            }
            await ReplaceLocalIfDifferent(newerRemoteHash);
            _trayMnu.SetLatestVersion(_fileKey, _filePath.GetVersion());
        }


        private async Task ReplaceLocalIfDifferent(string remoteFileSHA1)
        {
            if (SameHashes(remoteFileSHA1)) return;

            await DownloadAndWriteToDisk();

            await RunInitialCheck();
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
            Log(isSame ? "Local hash MATCHES that of remote."
                       : "Local hash DIFFERS from that of remote.");
            return isSame;
        }


        private async Task DownloadAndWriteToDisk()
        {
            Log("Downloading latest file from server ...");
            var b64 = await _client.GetContentB64(_fileKey);
            if (b64.IsBlank())
                Log($"Something went wrong at {nameof(DownloadAndWriteToDisk)}!");

            Log("Writing downloaded file to disk ...");
            DecodeB64ToDisk(b64);

            if (_fileKey == CheckerRelease.FileKey)
            {
                _listnr.Disconnect();
                CurrentExe.RelaunchApp();
            }
        }


        protected virtual void DecodeB64ToDisk(string b64)
        {
            var compressd = Path.GetTempFileName();
            b64.WriteBase64ToFile(compressd);
            compressd.LzmaDecodeAs(_filePath);
        }


        protected override Task ApplyChangesIfNeededAsync(BinaryFileChangeInfo change)
            => ReplaceLocalIfDifferent(change.NewSHA1);
    }
}
