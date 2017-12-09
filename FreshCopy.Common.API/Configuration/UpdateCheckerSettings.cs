﻿using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;

namespace FreshCopy.Common.API.Configuration
{
    public class UpdateCheckerSettings : IHubClientSettings
    {
        private string _savedFile;


        public string   ServerURL    { get; set; }
        public string   SharedKey    { get; set; }
        public string   UserAgent    { get; set; }
        public bool?    UpdateSelf   { get; set; }
        public bool?    CanExitApp   { get; set; }
        public string   LogglyToken  { get; set; }
        public string   FirebaseURL  { get; set; }
        public string   FirebaseKey  { get; set; }
        public string   FirebaseUsr  { get; set; }
        public string   FirebasePwd  { get; set; }


        public Dictionary<string, string>   BinaryFiles    { get; set; }
        public Dictionary<string, string>   AppendOnlyDBs  { get; set; }
        public Dictionary<string, string>   Executables    { get; set; }


        public static UpdateCheckerSettings CreateDefault() => new UpdateCheckerSettings
        {
            ServerURL   = "http://localhost:12345",
            UserAgent   = "sample client",
            SharedKey   = "abc123",
            //UpdateSelf  = true,
            //CanExitApp  = false,
            BinaryFiles = new Dictionary<string, string>
            {
                { "small text file", "smallText_targ.txt" },
                { "big text file", "bigText_targ.txt" },
            },
            AppendOnlyDBs = new Dictionary<string, string>
            {
                { "sample LiteDB 1", "sampleLiteDB1.LiteDB3" },
            },
            Executables = new Dictionary<string, string>
            {
                { "sample Exe 1", "sampleProgram1.exe" },
            }
        };

        public void   SetSavedFile  (string content) => _savedFile = content;
        public string ReadSavedFile () => _savedFile;
    }
}
