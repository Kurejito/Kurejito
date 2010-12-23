using System;
using System.ComponentModel;

namespace Kurejito.Gateways.PayPal
{
    /// <summary>
    /// Contains the information required to setup a specific PayPal environment (e.g. sandbox environment or live environment).
    /// </summary>
    public class PayPalEnvironment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalEnvironment"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="version"></param>
        /// <param name="accountCountry"></param>
        public PayPalEnvironment(string username, string password, string signature, Uri uri, string version, PayPalAccountCountry accountCountry)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (signature == null) throw new ArgumentNullException("signature");
            if (uri == null) throw new ArgumentNullException("uri");
            this.Username = username;
            this.Password = password;
            this.Signature = signature;
            Uri = uri;
            Version = version;
            AccountCountry = accountCountry;
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
        /// 
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Gets or sets the account country.
        /// </summary>
        /// <value>The account country.</value>
        public PayPalAccountCountry AccountCountry { get; private set; }

        /// <summary>
        /// </summary>
        internal static PayPalEnvironment NegativeTestAccountSandboxEnvironmentForGbr() {
            return NegativeTestAccountSandboxEnvironment(PayPalAccountCountry.GBR);
        }

        internal static PayPalEnvironment NegativeTestAccountSandboxEnvironment(PayPalAccountCountry palAccountCountry)
        {
            //TODO should probably remove this to prevent reflectoring people messing in our sandbox :)
            return new PayPalEnvironment("usguy_1290197714_biz_api1.bentaylor.org", "1290197724",
                                         "AFcWxV21C7fd0v3bYYYRCpSSRl31ArACdUVW.OGiJn8.H3UIaPI36X97", new Uri("https://api-3t.sandbox.paypal.com/nvp"), "56.0", palAccountCountry);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    //TODO maybe make this a general country enum and then validate when provided to gateways.
    public enum PayPalAccountCountry
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("United Kingdom")]
        GBR,
        /// <summary>
        /// 
        /// </summary>
        USA,
        /// <summary>
        /// 
        /// </summary>
        CAN
    }
/*
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    internal sealed class Iso3LetterCountryCodeAttribute : Attribute {
        public string Iso3LetterCountryCode { get; private set; }

        public Iso3LetterCountryCodeAttribute(string iso3LetterCountryCode) {
            if (iso3LetterCountryCode == null) throw new ArgumentNullException("iso3LetterCountryCode");
            this.Iso3LetterCountryCode = iso3LetterCountryCode.Trim().ToUpper();
            if (this.Iso3LetterCountryCode.Length != 3)
                throw new ArgumentOutOfRangeException("iso3LetterCountryCode", @"Must be exactly 3 characters.");
        }

        public static string GetCode(object o) {
            var customAttribute = Attribute.GetCustomAttribute(o.GetType(), typeof(Iso3LetterCountryCodeAttribute));
            return "";
        }
    }*/
}