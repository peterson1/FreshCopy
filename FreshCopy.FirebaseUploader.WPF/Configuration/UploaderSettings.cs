using CommonTools.Lib.ns11.GoogleTools;
using System.Collections.Generic;

namespace FreshCopy.FirebaseUploader.WPF.Configuration
{
    public class UploaderSettings
    {
        public string                      Filename       { get; set; }
        public string                      LogglyToken    { get; set; }
        public FirebaseCredentials         FirebaseCreds  { get; set; }
        public Dictionary<string, string>  PublicFiles    { get; set; }

        public override string ToString() => Filename;
    }
}
