using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;

namespace FreshCopy.Common.API.Configuration
{
    public class UpdateCheckerSettings : IHubClientSettings
    {

        public string   ServerURL   { get; set; }
        public string   SharedKey   { get; set; }
        public string   UserAgent   { get; set; }


        public Dictionary<string, string>   BinaryFiles    { get; set; }
        public Dictionary<string, string>   AppendOnlyDBs  { get; set; }


        public static UpdateCheckerSettings CreateDefault()
        {
            return new UpdateCheckerSettings
            {
                ServerURL   = "http://localhost:12345",
                UserAgent   = "sample client",
                SharedKey   = "abc123",
                BinaryFiles = new Dictionary<string, string>
                {
                    { "small text file", "smallText_targ.txt" },
                    { "big text file", "bigText_targ.txt" },
                },
                AppendOnlyDBs = new Dictionary<string, string>
                {
                    { "sample LiteDB 1", "sampleLiteDB1.LiteDB3" },
                }
            };
        }
    }
}
