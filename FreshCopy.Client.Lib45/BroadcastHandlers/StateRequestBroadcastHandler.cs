using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.SignalRClients;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.BroadcastHandlers
{
    public class StateRequestBroadcastHandler
    {
        private IMessageBroadcastListener _listnr;
        private bool _isBusy;


        public StateRequestBroadcastHandler(IMessageBroadcastListener messageBroadcastListener)
        {
            _listnr = messageBroadcastListener;
            _listnr.BroadcastReceived += OnBroadcastReceived;
        }


        private void OnBroadcastReceived(object sender, KeyValuePair<string, string> msg)
        {
            if (!IsRelevantMessage(msg.Value)) return;
            if (_isBusy) return;

            _isBusy = true;
            Task.Run(async () =>
            {
                var state = await GatherClientState();
                _listnr.SendClientState(state);
                _isBusy = false;
            });
        }


        private async Task<CurrentClientState> GatherClientState()
        {
            var state           = new CurrentClientState();
            state.PublicIP      = await GetPublicIP();
            state.ScreenshotB64 = GetScreenshotB64();
            return state;
        }


        private string GetScreenshotB64() { try
        {
            return CreateBitmap.FromPrimaryScreen()
                               .ConvertToBase64();
        }
        catch { return string.Empty; }}


        private async Task<string> GetPublicIP()
        {
            var hClient = new HttpClient();
            const string LOOKUP_URL = "https://api.ipify.org";
            try
            {
                return await hClient.GetStringAsync(LOOKUP_URL);
            }
            catch (TaskCanceledException)
            {
                return $"IP lookup timed out. (waited for {hClient.Timeout.Seconds} secs.)";
            }
            catch (Exception ex)
            {
                return ex.Info(true, true);
            }
        }


        private bool IsRelevantMessage(string message)
            => message == typeof(CurrentClientState).Name;
    }
}
