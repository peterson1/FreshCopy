using CommonTools.Lib.ns11.CollectionTools;
using CommonTools.Lib.ns11.GoogleTools;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CommonTools.Lib.fx45.GoogleTools
{
    public class GmailSender1
    {
        private WrappingIterator<GmailSenderSettings.Credentials> _accts;

        public GmailSender1(GmailSenderSettings gmailSenderSettings)
        {
            _accts = new WrappingIterator<GmailSenderSettings.Credentials>(gmailSenderSettings.Accounts);
        }


        public async Task Send(string recipientEmail, string subject, string messageBody)
        {
            var acct = _accts.GetNext();
            var from = new MailAddress(acct.Email, acct.SenderName);
            var dest = new MailAddress(recipientEmail);
            var smtp = CreateSmtpClient(acct);

            using (var msg = CreateMessage(from, dest, subject, messageBody))
            {
                await smtp.SendMailAsync(msg);
            }
        }


        private MailMessage CreateMessage(MailAddress from, MailAddress dest, string subject, string messageBody) 
            => new MailMessage(from, dest)
            {
                Subject = subject,
                Body    = messageBody
            };


        private SmtpClient CreateSmtpClient(GmailSenderSettings.Credentials creds) => new SmtpClient
        {
            Host                  = "smtp.gmail.com",
            Port                  = 587,
            EnableSsl             = true,
            DeliveryMethod        = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials           = new NetworkCredential(creds.Email, creds.Password)
        };
    }
}
