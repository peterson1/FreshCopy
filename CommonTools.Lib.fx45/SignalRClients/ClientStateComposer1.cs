using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.SignalRClients;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.SignalRClients
{
    public class ClientStateComposer1
    {
        public async Task<CurrentClientState> GatherClientState()
        {
            var state           = new CurrentClientState();
            state.ScreenshotB64 = GetScreenshotB64();
            state.PublicIP      = await GetPublicIP();
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
    }
}
