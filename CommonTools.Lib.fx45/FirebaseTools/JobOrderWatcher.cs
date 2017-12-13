using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.GoogleTools;
using System;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public class JobOrderWatcher : IDisposable
    {
        private FirebaseConnection  _conn;
        //private AgentStateUpdater   _stateUpdatr;

        private const string OBSERVABLE = "observable";
        private const string ROOTKEY    = "agents";
        private const string SUBKEY     = "JobOrder";


        public JobOrderWatcher(FirebaseConnection firebaseConnection,
                               //AgentStateUpdater agentStateUpdater,
                               FirebaseCredentials creds)
        {
            _conn        = firebaseConnection;
            //_stateUpdatr = agentStateUpdater;
        }


        public async Task StartWatching(Func<JobOrder, Task<JobResult>> jobToRun)
        {
            var path  = $"{ROOTKEY}/{_conn.AgentID}/{SUBKEY}";
            var found = await _conn.NodeFound(path, OBSERVABLE);
            if (!found) await _conn.CreateNode(new JobOrder
            {
                Command   = "Some Command",
                Requested = DateTime.Now,
                Started   = DateTime.Now,
            }, 
            path, OBSERVABLE);

            await _conn.AddSubscriber<JobOrder>(arg => RunMarked(arg, jobToRun), path);
        }


        private async Task RunMarked(JobOrder jo, Func<JobOrder, Task<JobResult>> jobToRun)
        {
            if (jo.Started.HasValue) return;

            jo.Started = DateTime.Now;
            jo.Result  = null;
            var path = $"{ROOTKEY}/{_conn.AgentID}/{SUBKEY}/{OBSERVABLE}";
            var desc = $"Executing “{jo.Command}”";
            await _conn.UpdateNode(jo, path);

            //await _stateUpdatr.SetRunningTask(_agentId, desc);

            try
            {
                jo.Result = await jobToRun(jo);
            }
            catch (Exception ex)
            {
                jo.Result = JobResult.Fail(desc, ex);
            }

            await _conn.UpdateNode(jo, path);
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
            }
            disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
