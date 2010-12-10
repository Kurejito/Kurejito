using System;

namespace Kurejito.Gateways.PayPal {
    /// <summary>
    /// Contains the information required to setup a specific PayPal environment (e.g. sandbox environment or live environment).
    /// </summary>
    public class PayPalEnvironment {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalEnvironment"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="uri">The URI.</param>
        public PayPalEnvironment(string username, string password, string signature, Uri uri) {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (signature == null) throw new ArgumentNullException("signature");
            if (uri == null) throw new ArgumentNullException("uri");
            this.Username = username;
            this.Password = password;
            this.Signature = signature;
            Uri = uri;
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

        ///<summary>
        ///</summary>
        public Uri Uri { get; private set; }

        /// <summary>
        /// </summary>
        internal static PayPalEnvironment NegativeTestAccountSandboxEnvironment() {
            //TODO should probably remove this to prevent reflectoring people messing in our sandbox :)
            return new PayPalEnvironment("usguy_1290197714_biz_api1.bentaylor.org", "1290197724",
                                         "AFcWxV21C7fd0v3bYYYRCpSSRl31ArACdUVW.OGiJn8.H3UIaPI36X97", new Uri("https://api-3t.sandbox.paypal.com/nvp"));
        }
    }
}