using CommonTools.Lib.ns11.SignalRHubServers;
using System.Collections.Generic;

namespace FreshCopy.Common.API.Configuration
{
    public class VersionKeeperSettings : ISignalRServerSettings
    {

        public string  ServerURL  { get; set; }


        public Dictionary<string, string>   BinaryFiles    { get; set; }
        public Dictionary<string, string>   AppendOnlyDBs  { get; set; }


        public static VersionKeeperSettings CreateDefault()
        {
            return new VersionKeeperSettings
            {
                ServerURL   = "http://localhost:12345",
                BinaryFiles = new Dictionary<string, string>
                {
                    { "small text file", "smallText_src.txt" },
                    { "big text file"  , "bigText_src.txt"   },
                },
                AppendOnlyDBs = new Dictionary<string, string>
                {
                    { "sample LiteDB 1", "sampleLiteDB1.LiteDB3" },
                }
            };
        }
    }
}
