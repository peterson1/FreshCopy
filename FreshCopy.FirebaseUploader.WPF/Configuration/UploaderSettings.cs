using CommonTools.Lib.ns11.GoogleTools;
using System.Collections.Generic;

namespace FreshCopy.FirebaseUploader.WPF.Configuration
{
    public class UploaderSettings
    {
        public FirebaseCredentials         FirebaseCreds  { get; set; }
        public Dictionary<string, string>  PublicFiles    { get; set; }
    }
}
