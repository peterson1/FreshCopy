using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.SignalRClients;
using CommonTools.Lib.ns11.SignalRServers;
using CommonTools.Lib.ns11.StringTools;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CommonTools.Lib.fx45.Cryptography
{
    public static class HMACRequestCypher1
    {
        private const string ENV_KEY     = "FC.UserId";
        private const string AUTH_HEADER = "session";
        private const string TIME_HEADER = "timestamp";


        public static void AddHmacCyphers(this IDictionary<string, string> headrs, IHubClientSettings cfg)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var authKey = GenerateAuthKey(cfg, timestamp);
            headrs.Add(AUTH_HEADER, authKey);
            headrs.Add(TIME_HEADER, timestamp);
        }


        private static string GenerateAuthKey(IHubClientSettings cfg, string timestamp)
        {
            var sessJson = ComposeSessionJson(cfg);
            var saltdKey = $"{timestamp}{cfg.SharedKey}".SHA1ForUTF8();
            return AESThenHMAC.SimpleEncryptWithPassword(sessJson, saltdKey);
        }


        private static string ComposeSessionJson(IHubClientSettings cfg)
        {
            var sess = new HubClientSession
            {
                UserAgent    = cfg.UserAgent,
                AgentVersion = CurrentExe.GetVersion(),
                ComputerName = Environment.MachineName,
                //JsonConfig   = JsonConvert.SerializeObject(cfg),
                JsonConfig   = cfg.ReadSavedFile(),
            };
            return JsonConvert.SerializeObject(sess);
        }


        public static bool TryGetSession(this IRequest request, out HubClientSession session)
        {
            if (request.Environment.TryGetValue(ENV_KEY, out object sessionObj))
            {
                session = sessionObj as HubClientSession;
                return session != null;
            }

            var timestamp = request.Headers[TIME_HEADER];
            var authHeadr = request.Headers[AUTH_HEADER];

            if (TryDecrypt(authHeadr, timestamp, out string json))
            {
                session = JsonConvert.DeserializeObject<HubClientSession>(json);
                request.Environment.Add(ENV_KEY, session);
                return true;
            }
            else
            {
                session = null;
                return false;
            }
        }


        public static bool TryGetSession(this HubCallerContext context, out HubClientSession session)
        {
            if (!context.Request.TryGetSession(out session)) return false;
            session.ConnectionId = context.ConnectionId;
            session.LastActivity = DateTime.Now;
            return true;
        }


        private static bool TryDecrypt(string encryptd, string timestamp, out string json)
        {
            if (encryptd.IsBlank() || timestamp.IsBlank())
            {
                json = null;
                return false;
            }
            var cfg = GlobalServer.Settings;
            var saltdKey = $"{timestamp}{cfg.SharedKey}".SHA1ForUTF8();
            json = AESThenHMAC.SimpleDecryptWithPassword(encryptd, saltdKey);
            return !json.IsBlank();
        }
    }
}
