using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.StringTools;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CommonTools.Lib.ns11.LoggingTools
{
    public static class Loggly
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string     _token;

        public static void SetToken(string customerToken) 
            => _token = customerToken;


        public static Task<HttpResponseMessage> Post(
            string message, 
            string tagPrefix = "LogglySender",
            [CallerMemberName] string callingMethod = null)
                => PostLog(message, tagPrefix, callingMethod);


        public static Task<HttpResponseMessage> Post(
            Exception exception,
            string tagPrefix = "LogglySender",
            [CallerMemberName] string callingMethod = null)
                => PostLog(exception.Info(true, true), 
                    tagPrefix, callingMethod);


        private static async Task<HttpResponseMessage> PostLog(string message, string tagPrefix, string callingMethod)
        {
            if (_token.IsBlank())
                throw Fault.CallFirst(nameof(SetToken));

            var tag = $"{tagPrefix}.{callingMethod}";
            var url = $"https://logs-01.loggly.com/inputs/{_token}/tag/{tag}";
            var txt = new StringContent(message);

            return await _client.PostAsync(url, txt);
        }
    }
}
