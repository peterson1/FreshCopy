﻿using CommonTools.Lib.ns11.GoogleTools;
using CommonTools.Lib.ns11.SignalRClients;
using System.Collections.Generic;

namespace FreshCopy.Common.API.Configuration
{
    public class UpdateCheckerSettings : IHubClientSettings
    {
        private string _savedFile;


        public string    ServerURL     { get; set; }
        public string    SharedKey     { get; set; }
        public string    UserAgent     { get; set; }
        public bool?     UpdateSelf    { get; set; }
        public bool?     CanExitApp    { get; set; }
        public string    LogglyToken   { get; set; }

        public FirebaseCredentials FirebaseCreds { get; set; }


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
            },
            FirebaseCreds = new FirebaseCredentials
            {
                BaseURL  = "https://your_firebase.firebaseio.com",
                ApiKey   = "yourApiKey",
                Email    = "email_if_using_email_auth",
                Password = "password_if_using_email_auth"
            },
        };

        public void   SetSavedFile  (string content) => _savedFile = content;
        public string ReadSavedFile () => _savedFile;
    }
}
