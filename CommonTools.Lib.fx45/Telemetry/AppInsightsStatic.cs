using System;
using System.Runtime.CompilerServices;

namespace CommonTools.Lib.fx45.Telemetry
{
    public static class AppInsights
    {
        private static AppInsightsClient _client;


        public static void StartTracking(string instrumentationKey)
            => _client = new AppInsightsClient(instrumentationKey);


        public static void Post(string eventName) { try
        {
            _client?.PostEvent(eventName);
        }
        catch { }}


        public static void Post(Exception ex, [CallerMemberName] string errorContext = null) { try
        {
            _client?.TrackNonFatalExceptions(ex, errorContext);
        }
        catch { }}


        public static void PageView(string pageName) { try
        {
            _client?.TrackPageView(pageName);
        }
        catch { }}


        public static bool IsTracking => _client != null;


        public static void Flush() { try
        {
            _client?.FlushData();
        }
        catch { }}
    }
}
