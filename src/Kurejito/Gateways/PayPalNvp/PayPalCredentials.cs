using System;
using System.Collections.Specialized;

namespace Kurejito.Gateways.PayPalNvp
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
            return new PayPalCredentials("sdk-three_api1.sdk.com", "QFZCWN5HZM8VBG7Q",
                                         "A‑IzJhZZjhg29XQ2qnhapuwxIDzyAZQ92FRP5dqBzVesOkzbdUONzmOU");
        }

        public void SetOn(NameValueCollection nameValueCollection)
        {
            if (nameValueCollection == null) throw new ArgumentNullException("nameValueCollection");
            
            //TODO if these need URL encoding too then will get rid of SetOn
            nameValueCollection["USER"] = Username;
            nameValueCollection["PWD"] = Password;
            nameValueCollection["SIGNATURE"] = Signature;
        }
    }
}