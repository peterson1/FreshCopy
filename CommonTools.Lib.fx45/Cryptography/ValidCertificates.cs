using CommonTools.Lib.ns11.StringTools;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CommonTools.Lib.fx45.Cryptography
{
    public static class ValidCertificates
    {
        private static HashSet<string> _whiteList = new HashSet<string>();


        public static void Include(string certificateThumb)
        {
            if (_whiteList.Count == 0)
            {
                ServicePointManager.ServerCertificateValidationCallback -= ValidationCallback;
                ServicePointManager.ServerCertificateValidationCallback += ValidationCallback;
            }

            _whiteList.Add(certificateThumb);
        }


        private static bool ValidationCallback(object sender, 
                                               X509Certificate certificate, 
                                               X509Chain chain, 
                                               SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;

            var x509Cert = certificate as X509Certificate2;
            if (x509Cert == null) return false;
            if (_whiteList.Count == 0) return false;

            var listed = _whiteList.Contains(x509Cert.Thumbprint);

            if (!listed)
            {
                var msg = $"Unlisted: {x509Cert.Thumbprint}" + L.f 
                    + "DnsName :  " + x509Cert.GetNameInfo(X509NameType.DnsName, true) + L.f
                    + "DnsFrom :  " + x509Cert.GetNameInfo(X509NameType.DnsFromAlternativeName, true) + L.f
                    + "EmailNa :  " + x509Cert.GetNameInfo(X509NameType.EmailName, true) + L.f
                    + "SimpleN :  " + x509Cert.GetNameInfo(X509NameType.SimpleName, true) + L.f
                    + "UpnName :  " + x509Cert.GetNameInfo(X509NameType.UpnName, true) + L.f
                    + "UrlName :  " + x509Cert.GetNameInfo(X509NameType.UrlName, true) + L.f
                    + "Issuer  :  " + x509Cert.Issuer + L.f
                    + "Subject :  " + x509Cert.Subject;

                throw new System.UnauthorizedAccessException(msg);
            }
            return listed;
        }
    }
}
