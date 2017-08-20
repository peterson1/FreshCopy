using CommonTools.Lib.ns11.SignalRHubServers;
using System.Collections.Generic;

namespace FreshCopy.Common.API.Configuration
{
    public class UpdateCheckerSettings : IHubClientSettings
    {

        public string   ServerURL  { get; set; }
        public string   HubName    { get; set; }


        public Dictionary<string, string>   BinaryFiles   { get; set; }


        public static UpdateCheckerSettings CreateDefault()
        {
            return new UpdateCheckerSettings
            {
                ServerURL   = "http://localhost:12345",
                HubName     = "VersionKeeperHub",
                BinaryFiles = new Dictionary<string, string>
                {
                    { "small text file", "smallText_targ.txt" },
                    { "big text file", "bigText_targ.txt" },
                }
            };
        }
    }
}
