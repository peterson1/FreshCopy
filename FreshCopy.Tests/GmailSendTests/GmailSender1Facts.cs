using CommonTools.Lib.fx45.FileSystemTools;
using CommonTools.Lib.fx45.GoogleTools;
using CommonTools.Lib.ns11.GoogleTools;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FreshCopy.Tests.GmailSendTests
{
    [Trait("Internet","local cfg")]
    public class GmailSender1Facts
    {
        //[Fact(DisplayName = "Solo Send")]
        //public async Task SoloSend()
        //{
        //    var cfg = JsonFile.Read<GmailSenderSettings>("gmailSender.cfg");
        //    var sut = new GmailSender1(cfg);
        //    var now = DateTime.Now.ToLongTimeString();
        //    var sbj = $"Sample subject {now}";
        //    var msg = $"Sample message {now}";
        //    await sut.Send("petersonsalamat@gmail.com", sbj, msg);
        //}


        [Fact(DisplayName = "Multi Send", Skip = "too spammy")]
        public async Task MultiSend()
        {
            var cfg = JsonFile.Read<GmailSenderSettings>("gmailSender.cfg");
            var sut = new GmailSender1(cfg);

            for (int i = 0; i < 6; i++)
            {
                var now = DateTime.Now.ToLongTimeString();
                var sbj = $"Sample subject {now}";
                var msg = $"Sample message {now}";
                await sut.Send("petersonsalamat@gmail.com", sbj, msg);
            }
        }
    }
}
