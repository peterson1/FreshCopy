using CommonTools.Lib.ns11.GoogleTools;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public class NewVersionWatcher : IDisposable
    {
        private FirebaseConnection  _conn;
        private FirebaseCredentials _creds;
        private string              _agentId;

        private const string OBSERVABLE = "observable";
        private const string ROOTKEY    = "agents";
        private const string SUBKEY     = "JobOrder";


        public NewVersionWatcher(FirebaseConnection firebaseConnection, 
                                 FirebaseCredentials creds)
        {
            _creds = creds;
            _conn  = firebaseConnection;
        }


        public async Task<bool> NewVersionInstalled()
        {
            await Task.Delay(1);
            return false;
        }


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                _conn?.Dispose();
                _conn  = null;
                _creds = null;
            }
            disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
