using System.Windows;

namespace FreshCopy.FirebaseUploader.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var json = JsonConvert.SerializeObject(new FirebaseCredentials
            //{
            //    BaseURL  = "https://your-acct.firebaseio.com",
            //    ApiKey   = "your-api-key",
            //    Email    = "your-email",
            //    Password = "your-password"
            //});
            //MessageBox.Show(AESThenHMAC.SimpleEncryptWithPassword
            //    (json, "your-instrumentation-key"));

            Components.Launch<MainWindow>(this);
        }
    }
}
