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
        private string              _agentId;

        private const string OBSERVABLE = "observable";
        private const string AGENTS    = "agents";
        private const string AGENT_STATE     = "AgentState";


        public AgentStateUpdater(FirebaseConnection firebaseConnection, 
                                 FirebaseCredentials creds)
        {
            _creds = creds;
            _conn  = firebaseConnection;
        }


        public Task SetRunningTask(string agentId, string desc)
            => _conn.UpdateNode(desc, GetPath(agentId), 
                nameof(AgentState.RunningTask));


        private string GetPath(string agentId)
            => $"{AGENTS}/{agentId}/{AGENT_STATE}/{OBSERVABLE}";


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
