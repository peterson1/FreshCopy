using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.GoogleTools;
using CommonTools.Lib.ns11.LoggingTools;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.FirebaseTools
{
    public class FirebaseConnection : IDisposable
    {
        private List<IDisposable> _observers = new List<IDisposable>();
        private FirebaseClient    _client;

        public bool IsConnected => _client != null;

        public async Task<bool> Open(FirebaseCredentials creds)
        {
            if (IsConnected) return true;
            try
            {
                var cf = new FirebaseConfig(creds.ApiKey);
                var ap = new FirebaseAuthProvider(cf);

                var au = await ap.SignInWithEmailAndPasswordAsync
                                (creds.Email, creds.Password);

                var op = new FirebaseOptions();
                op.AuthTokenAsyncFactory = async ()
                    => (await au.GetFreshAuthAsync()).FirebaseToken;

                _client = new FirebaseClient(creds.BaseURL, op);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> NodeFound(params string[] subPaths)
        {
            if (!IsConnected) throw Fault.CallFirst(nameof(Open));

            var node = await _client.Child(ToPath(subPaths))
                        .OnceSingleAsync<object>();

            return node != null;
        }


        public async Task CreateNode<T>(T obj, params string[] subPaths)
        {
            if (!IsConnected) throw Fault.CallFirst(nameof(Open));

            await _client.Child(ToPath(subPaths)).PutAsync(obj);
        }


        public Task UpdateNode<T>(T obj, params string[] subPaths)
            => CreateNode<T>(obj, subPaths);


        public async Task<bool> DeleteNode(params string[] subPaths)
        {
            if (!IsConnected) throw Fault.CallFirst(nameof(Open));

            var path = ToPath(subPaths);

            await _client.Child(path).DeleteAsync();

            return !(await NodeFound(path));
        }


        public  void AddSubscriber<T>(Func<T, Task> jobToRun, params string[] subPaths)
            => _observers.Add(
                     _client.Child(ToPath(subPaths))
                        .AsObservable<T>(OnSubscribeError)
                        .Subscribe(x => jobToRun(x.Object)));


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


        private string ToPath(params string[] subPaths)
            => string.Join("/", subPaths);


        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                if (_observers != null)
                {
                    foreach (var obs in _observers)
                        obs?.Dispose();

                    _observers = null;
                }
                _client = null;
            }
            disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
