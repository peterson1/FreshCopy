using CommonTools.Lib.ns11.GoogleTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.StringTools;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
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
        private List<IDisposable>   _observers = new List<IDisposable>();
        private FirebaseClient      _client;
        private FirebaseStorage     _storage;
        private FirebaseCredentials _creds;

        private const string UNSORTED = "unsorted";

        public bool IsConnected => _client != null;


        public FirebaseConnection(FirebaseCredentials firebaseCredentials)
        {
            _creds = firebaseCredentials;
            if (!_creds.Email.IsBlank())
                AgentID = _creds.Email.Replace("@", "_")
                                      .Replace(".", "_");
        }


        public string AgentID { get; set; }


        public async Task<bool> Open()
        {
            if (IsConnected) return true;
            try
            {
                var cf = new FirebaseConfig(_creds.ApiKey);
                var ap = new FirebaseAuthProvider(cf);

                var au = await ap.SignInWithEmailAndPasswordAsync
                                (_creds.Email, _creds.Password);

                Func<Task<string>> ts = async ()
                    => (await au.GetFreshAuthAsync()).FirebaseToken;

                var op = new FirebaseOptions();
                op.AuthTokenAsyncFactory = ts;

                _client = new FirebaseClient(_creds.BaseURL, op);

                var so = new FirebaseStorageOptions();
                so.AuthTokenAsyncFactory = ts;
                _storage = new FirebaseStorage(BucketURL, so);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private string BucketURL
            => _creds.BaseURL.Between("https://", 
                ".firebaseio.com") + ".appspot.com";


        public async Task<bool> NodeFound(params string[] subPaths)
        {
            var node = await GetNode<object>(subPaths);
            return node != null;
        }


        public async Task<T> GetNode<T>(params string[] subPaths)
        {
            if (!(await Open())) return default(T);
            return await _client.Child(ToPath(subPaths))
                                .OnceSingleAsync<T>();
        }


        public Task<string> GetText(params string[] subPaths)
            => GetNode<string>(subPaths);


        public async Task CreateNode<T>(T obj, params string[] subPaths)
        {
            if (!(await Open())) return;
            await _client.Child(ToPath(subPaths)).PutAsync(obj);
        }


        public Task UpdateNode<T>(T obj, params string[] subPaths)
            => CreateNode<T>(obj, subPaths);


        public async Task<bool> DeleteNode(params string[] subPaths)
        {
            if (!(await Open())) return false;

            var path = ToPath(subPaths);

            await _client.Child(path).DeleteAsync();

            return !(await NodeFound(path));
        }


        public async Task AddSubscriber<T>(Func<T, Task> jobToRun, params string[] subPaths)
        {
            if (!(await Open())) return;

            //_observers.Add(_client.Child(ToPath(subPaths))
            //                .AsObservable<T>(OnSubscribeError)
            //                .Subscribe(x => jobToRun(x.Object)));

            try
            {
                var observbl = _client.Child(ToPath(subPaths))
                                .AsObservable<T>(OnSubscribeError);
                var subscrptn = observbl.Subscribe
                    (_ => jobToRun(_.Object), ex => { }, () => { });

                _observers.Add(subscrptn);
            }
            catch (Exception) { }
        }


        public async Task<string> UploadFile(string filePath)
        {
            if (!(await Open())) return null;
            var fName = Path.GetFileName(filePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                return await _storage.Child(UNSORTED)
                                     .Child(fName)
                                     .PutAsync(stream);
            }
        }


        private void OnSubscribeError(object sender, ExceptionEventArgs<FirebaseException> e)
        {
            //if (e.Exception is FirebaseException fe)
            //{
            //    if (fe.InnerException is IOException io)
            //    {
            //        if (io.InnerException is WebException we && we.Message
            //            .Contains("The request was aborted: The request was canceled."))
            //            return;
            //    }
            //    else if (fe.InnerException is HttpRequestException ht)
            //    {
            //        if (ht.InnerException is WebException we && we.Message
            //            .Contains("The remote name could not be resolved"))
            //            return;
            //    }
            //}
            //Loggly.Post(e.Exception);
        }


        private string ToPath(params string[] subPaths)
            => string.Join("/", subPaths);


        #region IDisposable Support
        private bool _disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                if (_observers != null)
                {
                    foreach (var obs in _observers)
                        obs?.Dispose();
                }
                _observers = null;
                _client    = null;
                _storage   = null;
                _creds     = null;
            }
            _disposedValue = true;
        }
        public void Dispose() => Dispose(true);
        #endregion
    }
}
