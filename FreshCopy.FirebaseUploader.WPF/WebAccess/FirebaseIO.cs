using CommonTools.Lib.fx45.FirebaseTools;
using CommonTools.Lib.ns11.DataStructures;
using FreshCopy.FirebaseUploader.WPF.Configuration;
using System.Threading.Tasks;

namespace FreshCopy.FirebaseUploader.WPF.WebAccess
{
    public class FirebaseIO
    {
        private ConfigLoaderVM _loadr;
        private const string ROOT   = "files";
        private const string SUBKEY = "PublicFileInfo";

        public FirebaseIO(ConfigLoaderVM configLoaderVM)
        {
            _loadr = configLoaderVM;
        }


        internal Task<PublicFileInfo> GetInfo(string fileId)
            => Conn.GetNode<PublicFileInfo>(ROOT, fileId, SUBKEY);


        private FirebaseConnection Conn
            => new FirebaseConnection(_loadr.CurrentCfg.FirebaseCreds);
    }
}
