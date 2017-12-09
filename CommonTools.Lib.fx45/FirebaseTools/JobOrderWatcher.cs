using CommonTools.Lib.fx45.ThreadTools;
using CommonTools.Lib.ns11.DataStructures;
using CommonTools.Lib.ns11.LoggingTools;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public class JobOrderWatcher : IDisposable
    {
        private FirebaseClient _client;
        private IDisposable    _observr;
        private JobOrder       _cmd;
        private string         _baseUrl;
        private string         _apiKey;
        private string         _email;
        private string         _password;
        private string         _agentId;
        private string         _rootKey;
        private string         _subKey;


        public JobOrderWatcher(string baseUrl, string apiKey, 
            string email, string password, string agentId,
            string rootKey = "agents", string subKey = "JobOrder")
        {
            _baseUrl  = baseUrl;
            _apiKey   = apiKey;
            _email    = email;
            _password = password;
            _agentId  = agentId;
            _rootKey  = rootKey;
            _subKey   = subKey;
        }


        public async Task StartWatching(Func<JobOrder, Task<JobResult>> jobToRun)
        {
            _client  = await ConnectToFirebase();

            var found = await CommandNodeExists();
            if (!found) await CreateNewCommandNode();

            _observr = _client.Child(_rootKey).Child(_agentId)
                        .AsObservable<JobOrder>(OnSubscribeError)
                        .Subscribe(cmd => OnCommandChanged(cmd, jobToRun));
        }


        private async Task<bool> CommandNodeExists()
        {
            Dictionary<string, Dictionary<string, JobOrder>> root;

            try
            {
                root = await _client.Child(_rootKey)
                        .OnceSingleAsync<Dictionary<string, Dictionary<string, JobOrder>>>();
            }
            catch (FirebaseException ex) 
            when (ex.InnerException is JsonSerializationException)
            {
                return false;
            }

            if (root == null) return false;

            if (!root.ContainsKey(_agentId)) return false;
            var agentNode = root[_agentId];

            return agentNode.ContainsKey(_subKey);
        }


        private async Task CreateNewCommandNode()
        {
            await _client.Child(_rootKey).Child(_agentId).Child(_subKey)
                .PutAsync(new JobOrder
                {
                    Command   = "Some Command",
                    Requested = DateTime.Now,
                    Started   = DateTime.Now,
                });
        }


        private async Task<FirebaseClient> ConnectToFirebase()
        {
            var cf = new FirebaseConfig(_apiKey);
            var ap = new FirebaseAuthProvider(cf);
            var ok = await ap.SignInWithEmailAndPasswordAsync(_email, _password);
            var op = new FirebaseOptions();
            op.AuthTokenAsyncFactory = ()
                => Task.FromResult(ok.FirebaseToken);

            return new FirebaseClient(_baseUrl, op);
        }


        private void OnSubscribeError(object sender, ExceptionEventArgs<FirebaseException> e)
        {
            if (e.Exception is FirebaseException fe)
            {
                if (fe.InnerException is IOException io)
                {
                    if (io.InnerException is WebException we && we.Message
                        .Contains("The request was aborted: The request was canceled."))
                            return;
                }
                else if (fe.InnerException is HttpRequestException ht)
                {
                    if (ht.InnerException is WebException we && we.Message
                        .Contains("The remote name could not be resolved"))
                            return;
                }
            }
            Loggly.Post(e.Exception);
        }


        private async void OnCommandChanged(
            FirebaseEvent<JobOrder> eventObj,
            Func<JobOrder, Task<JobResult>> jobToRun)
        {
            _cmd = eventObj.Object;
            if (_cmd.Started.HasValue) return;

            await MarkCommandAsStarted();

            var result = await SafelyExecute(jobToRun);

            await WriteResults(result);
        }


        private async Task<JobResult> SafelyExecute(Func<JobOrder, Task<JobResult>> jobToRun)
        {
            try
            {
                return await jobToRun.Invoke(_cmd);
            }
            catch (Exception ex)
            {
                return JobResult.Fail(
                    $"Executing “{_cmd.Command}”", ex);
            }
        }


        private async Task MarkCommandAsStarted()
        {
            _cmd.Started = DateTime.Now;
            await _client.Child(_rootKey).Child(_agentId)
                .Child(_subKey).PutAsync(_cmd);
        }


        private async Task WriteResults(JobResult result)
        {
            _cmd.Result = result;
            await _client.Child(_rootKey).Child(_agentId)
                .Child(_subKey).PutAsync(_cmd);
        }


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                _observr?.Dispose();
                _observr = null;
                _client  = null;
                _cmd     = null;
            }
            disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
