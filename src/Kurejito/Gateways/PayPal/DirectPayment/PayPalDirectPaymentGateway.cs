using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kurejito.Payments;
using Kurejito.Transport;

namespace Kurejito.Gateways.PayPal.DirectPayment {
    /// <summary>
    ///   Responsible for processing payments using the Direct Payments functionality of PayPal Website Payments Pro.
    ///   See https://www.x.com/community/ppx/documentation#wpp
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

        private readonly PayPalEnvironment environment;
        private readonly IHttpPostTransport httpTransport;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PayPalDirectPaymentGateway" /> class.
        /// </summary>
        /// <param name = "httpTransport">The transport for PayPal communication.</param>
        /// <param name = "environment">The pay pal environment.</param>
        public PayPalDirectPaymentGateway(IHttpPostTransport httpTransport, PayPalEnvironment environment) {
            if (httpTransport == null) {
                throw new ArgumentNullException("httpTransport");
            }
            if (environment == null) {
                throw new ArgumentNullException("environment");
            }
            this.httpTransport = httpTransport;
            this.environment = environment;
        }

        #region IPurchaseGateway Members

        /// <summary>
        ///   Attempts to debit the specified amount from the supplied payment card.
        /// </summary>
        /// <param name = "merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
        /// <param name = "amount">The amount of money to be debited from the payment card</param>
        /// <param name = "currency">The ISO4217 currency code of the currency to be used for this transaction.</param>
        /// <param name = "card">An instance of <see cref = "PaymentCard" /> containing the customer's payment card details.</param>
        /// <returns>
        ///   A <see cref = "PaymentResponse" /> indicating whether the transaction succeeded.
        /// </returns>
        public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
            return this.Purchase(merchantReference, amount, currency, card, null);
        }

        /// <summary>
        ///   Attempts to debit the specified amount from the supplied payment card.
        /// </summary>
        /// <param name = "merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
        /// <param name = "amount">The amount of money to be debited from the payment card</param>
        /// <param name = "currency">The ISO4217 currency code of the currency to be used for this transaction.</param>
        /// <param name = "card">An instance of <see cref = "PaymentCard" /> containing the customer's payment card details.</param>
        /// <param name = "basket">An instance of <see cref = "Basket">Basket</see> containing descriptions of the items included in this transaction.</param>
        /// <returns>
        ///   A <see cref = "PaymentResponse" /> indicating whether the transaction succeeded.
        /// </returns>
        public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card, Basket basket) {
            if (merchantReference == null)
                throw new ArgumentNullException("merchantReference");
            if (currency == null)
                throw new ArgumentNullException("currency");
            if (card == null)
                throw new ArgumentNullException("card");

            ThrowIfAmountZeroOrLess(amount);

            ThrowIfCardNotSupportedByPayPal(card);

            var post = this.httpTransport.Post(this.environment.Uri, this.BuildPurchaseQueryString(card, amount, currency));

            //TODO support payment response and not the hack below.
            return new PaymentResponse {
                                           Reason = post,
                                           Status =
                                               post.Contains("ACK=Success") ? PaymentStatus.Ok : PaymentStatus.Error
                                       };
        }

        #endregion

        private string BuildPurchaseQueryString(PaymentCard card, decimal amount, string currency) {
            var pairs = new Dictionary<string, string> {
                                                           {"VERSION", "56.0"},
                                                           {"SIGNATURE", this.environment.Signature},
                                                           {"USER", this.environment.Username},
                                                           {"PWD", this.environment.Password},
                                                           {"METHOD", "DoDirectPayment"},//Required
                                                           {"PAYMENTACTION", "Sale"},//Other option is Authorization. Use when we do Auth and capture.
                                                           {"IPADDRESS", "192.168.1.1"},//TODO Required for fraud purposes.
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

            var values =
                pairs.Select(pair => String.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
            return (String.Join("&", values.ToArray()));
        }

        private static void ThrowIfCardNotSupportedByPayPal(PaymentCard card) {
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