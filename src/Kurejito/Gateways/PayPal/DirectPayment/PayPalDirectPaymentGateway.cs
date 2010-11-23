using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kurejito.Payments;
using Kurejito.Transport;

namespace Kurejito.Gateways.PayPal.DirectPayment {
    /// <summary>
    /// Responsible for processing payments using the Direct Payments functionality of PayPal Website Payments Pro.
    /// See https://www.x.com/community/ppx/documentation#wpp 
    /// </summary>
    public class PayPalDirectPaymentGateway : IPurchaseGateway {
        private static readonly IDictionary<CardType, string> SupportedCards = new Dictionary<CardType, string> {
                                                                                                                    {CardType.Visa, "Visa"},
                                                                                                                    {
                                                                                                                        CardType.Mastercard,
                                                                                                                        "MasterCard"
                                                                                                                        },
                                                                                                                    //TODO Other, Switch, Solo?
                                                                                                                };

        private readonly PayPalCredentials credentials;
        private readonly IHttpPostTransport httpTransport;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalDirectPaymentGateway"/> class.
        /// </summary>
        /// <param name="httpTransport">The transport for PayPal communication.</param>
        /// <param name="credentials">The pay pal credentials.</param>
        public PayPalDirectPaymentGateway(IHttpPostTransport httpTransport, PayPalCredentials credentials) {
            if (httpTransport == null) throw new ArgumentNullException("httpTransport");
            if (credentials == null) throw new ArgumentNullException("credentials");
            this.httpTransport = httpTransport;
            this.credentials = credentials;
        }

        #region IPurchaseGateway Members

        /// <summary>
        /// </summary>
        public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
            if (merchantReference == null) throw new ArgumentNullException("merchantReference");
            if (currency == null) throw new ArgumentNullException("currency");
            if (card == null) throw new ArgumentNullException("card");

            ThrowIfAmountZeroOrLess(amount);

            ThrowIfUnSupportedCardType(card);

            string post = this.httpTransport.Post(new Uri("https://api-3t.sandbox.paypal.com/nvp"),
                                                  this.BuildPurchaseQueryString(card, amount, currency));

            //TODO support payment response and not the hack below.
            return new PaymentResponse {
                                           Reason = post,
                                           Status = post.Contains("ACK=Success") ? PaymentStatus.OK : PaymentStatus.Error
                                       };
        }

        #endregion

        private string BuildPurchaseQueryString(PaymentCard card, decimal amount, string currency) {
            var pairs = new Dictionary<string, string> {
                                                           {"VERSION", "56.0"},
                                                           {"SIGNATURE", this.credentials.Signature},
                                                           {"USER", this.credentials.Username},
                                                           {"PWD", this.credentials.Password},
                                                           {"METHOD", "DoDirectPayment"},
                                                           {"PAYMENTACTION", "Sale"},
                                                           {"IPADDRESS", "192.168.1.1"},
                                                           {"AMT", amount.ToString("0.00")},
                                                           {"CREDITCARDTYPE", SupportedCards[card.CardType]},
                                                           {"ACCT", card.CardNumber},
                                                           {"EXPDATE", card.ExpiryDate.TwoDigitMonth + card.ExpiryDate.Year},
                                                           {"CVV2", card.CV2},
                                                           {"FIRSTNAME", "Bob"},
                                                           {"LASTNAME", "Le Builder"},
                                                           {"STREET", "1972 Toytown"},
                                                           {"CITY", "London"},
                                                           {"STATE", "London"},
                                                           {"ZIP", "N1 3JS"},
                                                           //TODO check how values for currency relate to PayPal currency codes
                                                           //https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_api_nvp_country_codes
                                                           {"COUNTRYCODE", "GB"},
                                                           {"CURRENCYCODE", currency}
                                                       };

            IEnumerable<string> values =
                pairs.Select(pair => String.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
            return (String.Join("&", values.ToArray()));
        }

        private static void ThrowIfUnSupportedCardType(PaymentCard card) {
            string ppCreditCardType;
            if (!SupportedCards.TryGetValue(card.CardType, out ppCreditCardType))
                throw new ArgumentException(string.Format("PaymentCard.CardType must be one of the following: {0}",
                                                          String.Join(" ",
                                                                      SupportedCards.Keys.Select(e => e.ToString()).
                                                                          ToArray())));
        }

        private static void ThrowIfAmountZeroOrLess(decimal amount) {
            if (amount <= 0)
                throw new ArgumentException("Purchase amount must be greater than zero.", "amount");
        }
    }
}