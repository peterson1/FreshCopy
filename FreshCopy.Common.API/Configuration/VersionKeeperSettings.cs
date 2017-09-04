using CommonTools.Lib.ns11.SignalRServers;
using System.Collections.Generic;

namespace FreshCopy.Common.API.Configuration
{
    public struct CheckerRelease
    {
        public const string FileKey = "FC.Checker.Release";
    }


    public class VersionKeeperSettings : ISignalRServerSettings
    {

        public string   ServerURL   { get; set; }
        public string   SharedKey   { get; set; }
        public string   MasterCopy  { get; set; }


        public Dictionary<string, string>   BinaryFiles    { get; set; }
        public Dictionary<string, string>   AppendOnlyDBs  { get; set; }


        public static VersionKeeperSettings CreateDefault()
        {
            return new VersionKeeperSettings
            {
                ServerURL   = "http://localhost:12345",
                SharedKey   = "abc123",
                MasterCopy  = @"c:\path\to\master\copy.exe",
                BinaryFiles = new Dictionary<string, string>
                {
                    { "small text file", "smallText_src.txt" },
                    { "big text file"  , "bigText_src.txt"   },
                    { CheckerRelease.FileKey, "path to official checker release" },
                },
                AppendOnlyDBs = new Dictionary<string, string>
                {
                    { "sample LiteDB 1", "sampleLiteDB1.LiteDB3" },
                }
            };
        }
    }
}
