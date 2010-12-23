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
    [Accepts("GBR", "AUD,CAD,CZK,DKK,EUR,HUF,JPY,NOK,NZD,PLN,GBP,SGD,SEK,CHF,USD", CardType.Visa, CardType.MasterCard)]
    [Accepts("GBR", "GBP", CardType.Visa, CardType.MasterCard, CardType.Solo, CardType.Maestro)]
    public class PayPalDirectPaymentGateway : IPurchase, IAuthoriseAndCapture {
        //TODO maybe make this map a type as in other providers too.
        private static readonly IDictionary<CardType, string> CardTypeToPayPalCardStringMap = new Dictionary<CardType, string> {
                                                                                                                      {CardType.Visa, "Visa"},
                                                                                                                      {CardType.MasterCard, "MasterCard"},
                                                                                                                      {CardType.Maestro, "Maestro"},
                                                                                                                      {CardType.Solo, "Solo"},
                                                                                                                      {CardType.Discover, "Discover"},
                                                                                                                  };

        private readonly PayPalEnvironment environment;
        private readonly IHttpPostTransport httpTransport;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PayPalDirectPaymentGateway" /> class.
        /// </summary>
        /// <param name = "httpTransport">The transport for PayPal communication.</param>
        /// <param name = "environment">The pay pal environment.</param>
        public PayPalDirectPaymentGateway(IHttpPostTransport httpTransport, PayPalEnvironment environment) {
            if (httpTransport == null) throw new ArgumentNullException("httpTransport");
            if (environment == null) throw new ArgumentNullException("environment");
            this.httpTransport = httpTransport;
            this.environment = environment;
        }

        #region IAuthoriseAndCapture Members

        public PaymentResponse Authorise(string merchantReference, Money amount, PaymentCard card) {
            if (merchantReference == null) throw new ArgumentNullException("merchantReference");
            if (card == null) throw new ArgumentNullException("card");
            ThrowIfFailPaymentChecks(amount, card);
            return ProcessResponse(this.Post(this.BuildDirectPaymentRequestMessage(card, amount, "Authorization")));
        }

        public PaymentResponse Capture() {
            throw new NotImplementedException();
        }

        #endregion

        #region IPurchase Members

        /// <summary>
        ///   Attempts to debit the specified amount from the supplied payment card.
        /// </summary>
        /// <param name = "merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
        /// <param name = "amount">The amount of money to be debited from the payment card (includes the ISO4217 currency code).</param>
        /// <param name = "card">An instance of <see cref = "PaymentCard" /> containing the customer's payment card details.</param>
        /// <returns>
        ///   A <see cref = "PaymentResponse" /> indicating whether the transaction succeeded.
        /// </returns>
        public PaymentResponse Purchase(string merchantReference, Money amount, PaymentCard card) {
            ThrowIfFailPaymentChecks(amount, card);
            return ProcessResponse(this.Post(this.BuildDirectPaymentRequestMessage(card, amount, "Sale")));
        }

        public bool Accepts(Currency currency, CardType cardType) {
            return AcceptsAttribute.DecoratedToAccept<PayPalDirectPaymentGateway>(this.environment.AccountCountry.ToString(), currency.Iso3LetterCode, cardType);
        }

        #endregion

        private void ThrowIfFailPaymentChecks(Money amount, PaymentCard card) {
            if(!Accepts(amount.Currency, card.CardType)) {
                throw new ArgumentOutOfRangeException("card", String.Format("Gateway cannot accept CardType of {0} with currency {1} in {2}.", card.CardType, amount.Currency.Iso3LetterCode, this.environment.AccountCountry));
            }

            if (amount <= 0)
                throw new ArgumentException(@"Purchase amount must be greater than zero.", "amount");

            string ppCreditCardType;
            if (!CardTypeToPayPalCardStringMap.TryGetValue(card.CardType, out ppCreditCardType))
                throw new ArgumentException(string.Format("PaymentCard.CardType must be one of the following: {0}", String.Join(" ", CardTypeToPayPalCardStringMap.Keys.Select(e => e.ToString()).ToArray())));
        }

        private static PaymentResponse ProcessResponse(string response) {
            var nameValueCollection = HttpUtility.ParseQueryString(response);

            var ack = nameValueCollection["ACK"];

            if (ack.Equals("Success") || ack.Equals("SuccessWithWarning")) //TODO reflect PartialSuccess in the PaymentResponse.
                return new PaymentResponse {
                                               PaymentId = nameValueCollection["TRANSACTIONID"],
                                               Status = PaymentStatus.Ok,
                                               Reason = String.Empty
                                           };

            if (ack.Equals("PartialSuccess"))
                throw new NotSupportedException("Received PartialSuccess Ack from PayPal.  This should only be returned for parallel payments (which we don't support).");

            if (ack.Equals("Failure") || ack.Equals("FailureWithWarning") || ack.Equals("Warning"))
                return new PaymentResponse {
                                               Reason = String.Empty,
                                               //TODO look at if we need to handle more than one L_SHORTMESSAGE
                                               Status = StatusFromShortMessage(nameValueCollection["L_SHORTMESSAGE0"])
                                           };

            throw new InvalidOperationException(string.Format("Unknown Ack {0} received from PayPal.", ack));
        }

        private static PaymentStatus StatusFromShortMessage(string shortMessage) {
            if (shortMessage.Equals("Gateway Decline"))
                return PaymentStatus.Declined;
            throw new NotImplementedException(string.Format("We have not implemented status for L_SHORTMESSAGE0 of {0}.", shortMessage));
        }

        private string BuildDirectPaymentRequestMessage(PaymentCard card, Money amount, string paymentAction) {
            var pairs = new Dictionary<string, string> {
                                                           {"VERSION", this.environment.Version},
                                                           {"SIGNATURE", this.environment.Signature},
                                                           {"USER", this.environment.Username},
                                                           {"PWD", this.environment.Password},
                                                           {"METHOD", "DoDirectPayment"}, //Required
                                                           {"PAYMENTACTION", paymentAction}, //Other option is Authorization. Use when we do Auth and capture.
                                                           {"IPADDRESS", "192.168.1.1"}, //TODO Required for fraud purposes.
                                                           {"AMT", amount.ToString("0.00")},
                                                           {"CREDITCARDTYPE", CardTypeToPayPalCardStringMap[card.CardType]},
                                                           {"ACCT", card.CardNumber},
                                                           {"EXPDATE", card.ExpiryDate.TwoDigitMonth + card.ExpiryDate.Year},
                                                           {"CVV2", card.CV2},
                                                           //TODO billing address data for PayPal.
                                                           {"FIRSTNAME", "Bob"},
                                                           {"LASTNAME", "Le Builder"},
                                                           {"STREET", "1972 Toytown"},
                                                           {"CITY", "London"},
                                                           {"STATE", "London"},
                                                           {"ZIP", "N1 3JS"},
                                                           //TODO check how values for currency relate to PayPal currency codes
                                                           //https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_api_nvp_country_codes
                                                           {"COUNTRYCODE", "GB"}, //TODO
                                                           {"CURRENCYCODE", amount.Currency.Iso3LetterCode}
                                                       };

            var values =
                pairs.Select(pair => String.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
            return (String.Join("&", values.ToArray()));
        }

        private string Post(string message) {
            return this.httpTransport.Post(this.environment.Uri, message);
        }
    }
}