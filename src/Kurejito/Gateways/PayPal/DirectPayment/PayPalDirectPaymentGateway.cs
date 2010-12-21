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
    public class PayPalDirectPaymentGateway : IPurchase, IAuthoriseAndCapture, IAccept {

        private static readonly IList<AcceptEntry> UkAccepts = new List<AcceptEntry> 
        {
            new AcceptEntry(Currency.Aud, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Cad, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Czk, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Dkk, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Eur, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Huf, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Jpy, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Nok, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Nzd, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Pln, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Gbp, CardType.Visa, CardType.MasterCard, CardType.Maestro, CardType.Solo),
            new AcceptEntry(Currency.Sgd, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Sek, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Chf, CardType.Visa, CardType.MasterCard),
            new AcceptEntry(Currency.Usd, CardType.Visa, CardType.MasterCard),
        };

        //MAYBE add SupportedCards to the public interface? Then one can query for a processor to match certain criteria.
        //TODO if we add support for Canada we need to switch out supported cards based on Country in the PayPalEnvironment or similar.
        private static readonly IDictionary<CardType, string> UkSupportedCards = new Dictionary<CardType, string> {
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
            throw new NotImplementedException();
        }

        #endregion

        private static void ThrowIfFailPaymentChecks(Money money, PaymentCard card) {
            //TODO add supported currencies check (and add common interface so we can query providers).
            //TODO the supported cards stuff could be SRP'd for reuse.

            if (money <= 0)
                throw new ArgumentException(@"Purchase amount must be greater than zero.", "amount");

            string ppCreditCardType;
            if (!UkSupportedCards.TryGetValue(card.CardType, out ppCreditCardType))
                throw new ArgumentException(string.Format("PaymentCard.CardType must be one of the following: {0}", String.Join(" ", UkSupportedCards.Keys.Select(e => e.ToString()).ToArray())));
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
                                                           {"CREDITCARDTYPE", UkSupportedCards[card.CardType]},
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