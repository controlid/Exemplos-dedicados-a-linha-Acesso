using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ControliD
{
    public static class SSLValidator
    {
        private static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void OverrideValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
            ServicePointManager.Expect100Continue = false;
            // O Rep não Aceita!
            // throw HTTPErrorException(417, "Expectation failed"); // Request.cpp
        }
    }
}
