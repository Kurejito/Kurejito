using System;

namespace Kurejito.Gateways.PayPal
{
    public class PayPalCredentials
    {
        public PayPalCredentials(string username, string password, string signature)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (signature == null) throw new ArgumentNullException("signature");
            Username = username;
            Password = password;
            Signature = signature;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Signature { get; private set; }

        public static PayPalCredentials CreateSampleCredentials()
        {
            return new PayPalCredentials("usguy_1290197714_biz_api1.bentaylor.org", "1290197724",
                                         "AFcWxV21C7fd0v3bYYYRCpSSRl31ArACdUVW.OGiJn8.H3UIaPI36X97");
        }
    }
}