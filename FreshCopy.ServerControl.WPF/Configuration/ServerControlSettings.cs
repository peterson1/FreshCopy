using CommonTools.Lib.ns11.SignalRClients;

namespace FreshCopy.ServerControl.WPF.Configuration
{
    class ServerControlSettings : IHubClientSettings
    {
        private string _savedFile;


        public string   ServerURL   { get; set; }
        public string   SharedKey   { get; set; }
        public string   UserAgent   { get; set; }


        internal static ServerControlSettings CreateDefault() => new ServerControlSettings
        {
            ServerURL = "http://localhost:12345",
            UserAgent = "sample client",
            SharedKey = "abc123",
        };

        public void   SetSavedFile  (string content) => _savedFile = content;
        public string ReadSavedFile () => _savedFile;
    }
}
