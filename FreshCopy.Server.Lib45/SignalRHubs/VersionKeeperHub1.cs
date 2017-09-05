using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.fx45.Cryptography;
using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.LiteDbTools;
using CommonTools.Lib.fx45.LoggingTools;
using CommonTools.Lib.fx45.SignalRServers;
using CommonTools.Lib.fx45.ViewModelTools;
using CommonTools.Lib.ns11.SignalRClients;
using FreshCopy.Common.API.Configuration;
using FreshCopy.Common.API.HubServers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FreshCopy.Server.Lib45.SignalRHubs
{
    [AuthorizeV1]
    [HubName(VersionKeeperHub.Name)]
    public class VersionKeeperHub1 : Hub, IVersionKeeperServer
    {
        private VersionKeeperSettings _cfg;
        private SharedLogListVM       _logs;
        private CurrentHubClientsVM   _clients;


        public VersionKeeperHub1(VersionKeeperSettings versionKeeperSettings,
                                 SharedLogListVM sharedLogListVM,
                                 CurrentHubClientsVM currentHubClientsVM)
        {
            _cfg     = versionKeeperSettings;
            _logs    = sharedLogListVM;
            _clients = currentHubClientsVM;
        }


        public async Task<string> GetContentB64(string fileKey)
        {
            await Task.Delay(0);
            if (!IsValidBinaryKey(fileKey, out string filePath)) return string.Empty;
            try
            {
                return filePath.LzmaEncodeThenBase64();
            }
            catch (Exception ex)
            {
                _logs.Add(ex);
                return string.Empty;
            }
        }


        public async Task<string> GetLatestSHA1(string fileKey)
        {
            await Task.Delay(0);
            if (!IsValidBinaryKey(fileKey, out string filePath)) return string.Empty;
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


        public async Task<long> GetMaxId(string fileKey)
        {
            await Task.Delay(0);
            if (!IsValidDbKey(fileKey, out string filePath)) return -1;
            try
            {
                return AnyLiteDB.GetMaxId(filePath);
            }
            catch (Exception ex)
            {
                _logs.Add(ex);
                return -1;
            }
        }


        public async Task<List<string>> GetRecords(string fileKey, long startId)
        {
            await Task.Delay(0);
            if (!IsValidDbKey(fileKey, out string filePath)) return null;
            try
            {
                return AnyLiteDB.GetRecords(filePath, startId);
            }
            catch (Exception ex)
            {
                _logs.Add(ex);
                return null;
            }
        }


        private bool IsValidBinaryKey(string fileKey, out string filePath)
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


        private bool IsValidDbKey(string fileKey, out string filePath)
        {
            if (!_cfg.AppendOnlyDBs.TryGetValue(fileKey, out filePath))
            {
                Log($"Unrecognized AppendOnlyDBs key: “{fileKey}”");
                return false;
            }
            if (!File.Exists(filePath))
            {
                Log($"Missing database file: {filePath}");
                return false;
            }
            return true;
        }


        public override async Task OnConnected()
        {
            await Task.Delay(0);
            if (!IsValidSession(out HubClientSession session)) return;
            _clients.Add(session);
            await base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            _clients.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        private void Log(string message) => _logs.Add(message);


        private bool IsValidSession(out HubClientSession session)
        {
            if (!Context.Request.TryGetSession(out session)) return false;
            session.ConnectionId = Context.ConnectionId;
            session.LastActivity = DateTime.Now;
            session.HubName      = VersionKeeperHub.Name;
            return true;
        }
    }
}
