using System;

namespace Kurejito.Gateways.PayPal
{
    /// <summary>
    /// Represents the information needed to access a PayPal account.
    /// </summary>
    public class PayPalCredentials
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalCredentials"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="signature">The signature.</param>
        public PayPalCredentials(string username, string password, string signature)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (signature == null) throw new ArgumentNullException("signature");
            Username = username;
            Password = password;
            Signature = signature;
        }

        ///<summary>
        ///</summary>
        public string Username { get; private set; }
        ///<summary>
        ///</summary>
        public string Password { get; private set; }
        ///<summary>
        ///</summary>
        public string Signature { get; private set; }

        /// <summary>
        /// </summary>
        public static PayPalCredentials CreateSampleCredentials()
        {
            return new PayPalCredentials("usguy_1290197714_biz_api1.bentaylor.org", "1290197724",
                                         "AFcWxV21C7fd0v3bYYYRCpSSRl31ArACdUVW.OGiJn8.H3UIaPI36X97");
        }
    }
}