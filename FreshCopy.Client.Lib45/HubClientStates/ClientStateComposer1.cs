using CommonTools.Lib.fx45.ByteCompression;
using CommonTools.Lib.fx45.ImagingTools;
using CommonTools.Lib.ns11.ExceptionTools;
using CommonTools.Lib.ns11.LoggingTools;
using CommonTools.Lib.ns11.SignalRClients;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FreshCopy.Client.Lib45.HubClientStates
{
    public class ClientStateComposer1
    {
        private IMessageBroadcastClient _client;

        public ClientStateComposer1(IMessageBroadcastClient messageBroadcastClient)
        {
            _client = messageBroadcastClient;
        }


        public async Task<CurrentClientState> GatherClientState()
        {
            var state           = new CurrentClientState();
            state.ScreenshotB64 = await GetScreenshotB64();
            state.PublicIP      = (await GetPublicIP()).Trim();
            state.PrivateIPs    = GetPrivateIPs();
            return state;
        }


        private string GetPrivateIPs()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return "network unavailable";

            try
            {
                var entry = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                    .FirstOrDefault(_ => _.AddressFamily == AddressFamily.InterNetwork);

                return entry?.ToString() ?? "No InterNetwork NIC found.";
            }
            catch (Exception ex)
            {
                return ex.Info(true, true);
            }
        }


        //private string GetScreenshotB64() { try
        //{
        //    return CreateBitmap.FromPrimaryScreen()
        //                       .ConvertToBase64();
        //}
        //catch { return string.Empty; }}

        private async Task<string> GetScreenshotB64()
        {
            try
            {
                var bmp = CreateBitmap.FromPrimaryScreen();
                var tmp = bmp.ToTempPNG();
                var b64 = tmp.LzmaEncodeThenB64();
                File.Delete(tmp);
                return b64;
            }
            catch (Exception ex)
            {
                //_client.SendException("GetScreenshotB64", ex);
                await Loggly.Post(ex);
                return string.Empty;
            }
        }


        private async Task<string> GetPublicIP()
        {
            var hClient = new HttpClient();
            //https://stackoverflow.com/a/3097641/3973863
            //const string LOOKUP_URL = "https://api.ipify.org";
            //const string LOOKUP_URL = "http://checkip.dyndns.org/";
            const string LOOKUP_URL = "http://ifconfig.me/ip";
            try
            {
                return await hClient.GetStringAsync(LOOKUP_URL);
            }
            catch (TaskCanceledException)
            {
                return $"IP lookup timed out. (waited for {hClient.Timeout.Seconds} secs.)";
            }
            catch (HttpRequestException ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Info(true, true);
            }
        }
    }
}
