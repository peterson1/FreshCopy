using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.GoogleTools;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public class AgentStateUpdater : IDisposable
    {
        private FirebaseConnection  _conn;
        private FirebaseCredentials _creds;

        private const string AGENTS      = "agents";
        private const string AGENT_STATE = "AgentState";


        public AgentStateUpdater(FirebaseConnection firebaseConnection,
                                 FirebaseCredentials firebaseCredentials)
        {
            _conn   = firebaseConnection;
            _creds  = firebaseCredentials;
        }



        public Task SetState(string taskDesc, string exeSHA1 = null, string exeVersion = null)
        {
            var exe = CurrentExe.GetFullPath();
            return _conn.UpdateNode(new AgentState
            {
                RunningTask   = taskDesc,
                LastActivity  = DateTime.Now,
                ExeSHA1       = exeSHA1 ?? exe.SHA1ForFile(),
                ExeVersion    = exeVersion ?? exe.GetVersion(),
            },
            NodePath);
        }


        public Task<AgentState> GetState()
            => _conn.GetNode<AgentState>(NodePath);


        public Task SetRunningTask(string desc)
            => _conn.UpdateNode(desc, 
                                NodePath, 
                                nameof(AgentState.RunningTask));


        public Task SetExeVersion()
            => _conn.UpdateNode(CurrentExe.GetVersion(), 
                                NodePath,
                                nameof(AgentState.ExeVersion));


        private string NodePath
            => $"{AGENTS}/{_conn.AgentID}/{AGENT_STATE}";


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                _conn?.Dispose();
                _conn  = null;
            }
            disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
