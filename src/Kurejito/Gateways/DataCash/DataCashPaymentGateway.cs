using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Kurejito.Payments;
using Kurejito.Transport;

///<summary>Classes that implement the Kurejito API on top of the payment gateway supplied by <a href="DataCash">DataCash</a></summary>
namespace Kurejito.Gateways.DataCash {
	/// <summary>Implements the <see cref="IPurchaseGateway" /> interface to provide purchase and immediate payment 
	/// capabilities when using DataCash as your payment provider.</summary>
	public class DataCashPaymentGateway : IPurchaseGateway {
		private readonly string client;
		private readonly IHttpPostTransport http;
		private readonly string password;

		private readonly string gatewayUri;
		/// <summary>
		/// Construct a new instance of the <see cref="DataCashPaymentGateway"/>.
		/// </summary>
		/// <param name="http">The Http transport provider for communication with DataCash.</param>
		/// <param name="client">The client code for your payment gateway, as supplied by DataCash.</param>
		/// <param name="password">The password for your payment gateway, as supplised by DataCash.</param>
		/// <param name="gatewayUri">The endpoint URI of the DataCash payment system.</param>
		public DataCashPaymentGateway(IHttpPostTransport http, string client, string password, string gatewayUri) {
			this.http = http;
			this.client = client;
			this.password = password;
			this.gatewayUri = gatewayUri;
		}

		/// <summary>
		/// </summary>
		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
			var xml = new XDocument(
				new XDeclaration("1.0", "UTF-8", null),
				new XElement("Request",
							 MakeAuthenticationElement(),
							 MakeTransactionElement(merchantReference, amount, currency, card, Method.auth)
					)
				);
			var response = http.Post(new Uri(gatewayUri),
										xml.ToString(SaveOptions.DisableFormatting));
			var xmlResponse = XDocument.Parse(response);
			var status = this.ExtractStatus(xmlResponse);
			return (new PaymentResponse {
				Status = status,
				Reason = response
			});
		}

		private XElement MakeAuthenticationElement() {
			return (new XElement("Authentication", new XElement("client", client), new XElement("password", password)));
		}

		private XElement MakeCardTxnElement(PaymentCard card, Method method) {
			var cardElement = new XElement("Card",
										   new XElement("pan", card.CardNumber),
										   new XElement("expirydate", card.ExpiryDate.MM_YY)
				);
			if (card.HasStartDate) cardElement.Add(new XElement("startdate", card.StartDate.MM_YY));
			if (card.HasIssueNumber) cardElement.Add(new XElement("issuenumber", card.IssueNumber));

			var cardTxnElement = new XElement("CardTxn",
											  cardElement,
											  new XElement("method", method)
				);
			return (cardTxnElement);
		}

		private XElement MakeTxnDetailsElement(string merchantreference, decimal amount, string currency) {
			return (new XElement("TxnDetails",
								 new XElement("merchantreference", merchantreference),
								 new XElement("amount", new XAttribute("currency", currency), amount.ToString("0.00"))
				));
		}

		private XElement MakeTransactionElement(string merchantreference, decimal amount, string currency,
												PaymentCard card, Method method) {
			return (new XElement("Transaction",
								 MakeCardTxnElement(card, method),
								 MakeTxnDetailsElement(merchantreference, amount, currency)
				));
		}

		// ReSharper disable InconsistentNaming
		/// <summary>Payment methods - enumeration members must match exact values in DataCash API.
		/// 
		/// </summary>
		private enum Method {
			auth,
			pre,
			refund,
			erp
		}
		// ReSharper restore InconsistentNaming


		private PaymentStatus ExtractStatus(XNode xml) {
			var returnCodeElement = xml.XPathSelectElement("Response/status");
			var returnCode = 0;
			if (Int32.TryParse(returnCodeElement.Value, out returnCode)) {
				switch (returnCode) {
					#region Big tedious list of DataCash return values taken from https://testserver.datacash.com/software/returncodes.shtml

					// Success Transaction accepted and logged.
					case 1: return (PaymentStatus.Ok);
					// Socket write error Communication was interrupted. The argument is e.g. 523/555 (523 bytes written but 555 expected).
					case 2: return (PaymentStatus.Error); ;
					// Timeout A timeout occurred while we were reading the transaction details.
					case 3: return (PaymentStatus.Error); ;
					// Edit error A field was specified twice, you sent us too much or invalid data, a pre-auth lookup failed during a fulfill transaction, the swipe field was incorrectly specified, or you omitted a field. The argument will give a better indication of what exactly went wrong.
					case 5: return (PaymentStatus.Invalid);
					// Comms error Error in communications link; resend.
					case 6: return (PaymentStatus.Error);
					// Not authorised Transaction declined. The arguments are as return code 1, except the first argument is the bank's reason for declining it (e.g. REFERRAL, CALL AUTH CENTRE, PICK UP CARD etc.) or the result from a failed fraud check (e.g. FRAUD DECLINED reason code)
					case 7: return (PaymentStatus.Declined);
					// Currency error The currency you specified does not exist
					case 9: return (PaymentStatus.Invalid);
					// Authentication error The vTID or password were incorrect
					case 10: return (PaymentStatus.Error);
					// Invalid authorisation code The authcode you supplied was invalid
					case 12: return (PaymentStatus.Invalid);
					// Type field missing You did not supply a transaction type.
					case 13: return (PaymentStatus.Invalid);
					// Database server error Transaction details could not be committed to our database.
					case 14: return (PaymentStatus.Error);
					// Invalid type You specified an invalid transaction type
					case 15: return (PaymentStatus.Invalid);
					// Cannot fulfill transaction You attempted to fulfill a transaction that either could not be fulfilled (e.g. auth, refund) or already has been.
					case 19: return (PaymentStatus.Invalid);
					// Duplicate transaction reference A successful transaction has already been sent using this vTID and reference number.
					case 20: return (PaymentStatus.Invalid);
					// Invalid card type This terminal does not accept transactions for this type of card (e.g. Diner's Club, American Express if the merchant does not take American Express, Domestic Maestro if multicurrency only).
					case 21: return (PaymentStatus.Invalid);
					// Invalid reference Reference numbers should be 16 digits for fulfill transactions, or between 6 and 30 digits for all others.
					case 22: return (PaymentStatus.Invalid);
					// Expiry date invalid The expiry dates should be specified as MM/YY or MM-YY.
					case 23: return (PaymentStatus.Invalid);
					// Card has already expired The supplied expiry date is in the past.
					case 24: return (PaymentStatus.Invalid);
					// Card number invalid The card number does not pass the standard Luhn checksum test.
					case 25: return (PaymentStatus.Invalid);
					// Card number wrong length The card number does not have the expected number of digits.
					case 26: return (PaymentStatus.Invalid);
					// Issue number error You did not supply an issue number when we expected one, or the issue number you supplied was non-numeric or too long
					case 27: return (PaymentStatus.Invalid);
					// Start date error The start date was missing or malformed (must be MM/YY)
					case 28: return (PaymentStatus.Invalid);
					// Card is not valid yet The supplied start date is in the future
					case 29: return (PaymentStatus.Invalid);
					// Start date after expiry date 
					case 30: return (PaymentStatus.Invalid);
					// Invalid amount The amount is missing, is not fully specified to x.xx format
					case 34: return (PaymentStatus.Invalid);
					// Invalid cheque type Must be either "business" or "personal"
					case 40: return (PaymentStatus.Invalid);
					// Invalid cheque number Cheque number was missing or was not 6 digits
					case 41: return (PaymentStatus.Invalid);
					// Invalid sort code The sort code was missing or was not 6 digits
					case 42: return (PaymentStatus.Invalid);
					// Invalid account number The account number was missing or was not 8 digits
					case 44: return (PaymentStatus.Invalid);
					// Reference in use A transaction with this reference number is already going through the system.
					case 51: return (PaymentStatus.Invalid);
					// No free TIDs available for this vTID There are matching TIDs available, but they are all in use.
					case 53: return (PaymentStatus.Error);
					// Card used too recently 
					case 56: return (PaymentStatus.Invalid);
					// Invalid velocity_check value The velocity_check value must be numeric and between 0 and 120
					case 57: return (PaymentStatus.Invalid);
					// This combination of currency, card type and environment is not supported by this vTID You either:
					//  do not take the card type at all (eg. Amex card when you have no Amex TIDs)
					//  take this currency for some card types, but not this one. (e.g. you wanted to process Domestic Maestro in USD)
					//  do not take this card type and currency combination for the specified environment. (e.g. you specified capturemethod of 'cnp' when you are only setup for 'ecomm' transactions, or you specified capturemethod of 'cont_auth' or attempted a Historic CA transaction when you are not set up for CA transactions for the supplied card type and currency)
					case 59: return (PaymentStatus.Invalid);
					// Invalid XML The XML Document is not valid with our Request schema. The reason is detailed in the <information> element of the Response document.
					case 60: return (PaymentStatus.Invalid);
					// Configuration error An error in account configuration caused the transaction to fail. Contact DataCash Technical Support
					case 61: return (PaymentStatus.Error);
					// Unsupported protocol Please use the DataCash XML API
					case 62: return (PaymentStatus.Error);
					// Method not supported by acquirer The transaction type is not supported by the Acquirer
					case 63: return (PaymentStatus.Invalid);
					// APACS30: WRONG TID Error in bank authorization, where APACS30 Response message refers to different TID to that used in APACS30 Request message; resend.
					case 104: return (PaymentStatus.Error);
					// APACS30: MSG SEQ NUM ERR Error in bank authorization, where APACS30 Response message refers to different message number to that used in APACS30 Request message; resend.
					case 105: return (PaymentStatus.Error);
					// APACS30: WRONG AMOUNT Error in bank authorization, where APACS30 Response message refers to different amount to that used in APACS30 Request message; resend.
					case 106: return (PaymentStatus.Error);
					// No capture method specified Your vTID is capable of dealing with transactions from different environments (e.g. MoTo, e-comm), but you have not specified from which environment this transaction has taken place.
					case 190: return (PaymentStatus.Error);
					// Unknown format of datacash reference The datacash reference should be a 16 digit number. The first digit (2, 9, 3 or 4) indicates the format used and whether the txn was processed in a live or test environment.
					case 280: return (PaymentStatus.Invalid);
					// Datacash reference fails Luhn check The new format of datacash reference includes a luhn check digit. The number supplied failed to pass the luhn check.
					case 281: return (PaymentStatus.Invalid);
					// Mismatch between historic and current site_id The site_id extracted from the datacash reference does not match the current environment
					case 282: return (PaymentStatus.Invalid);
					// Mismatch between historic and current modes The mode flag extracted from the datacash reference does not match the current environment
					case 283: return (PaymentStatus.Invalid);
					// Payment Gateway Busy Out of external connections
					case 440: return (PaymentStatus.Error);
					// Maestro txns for CNP not supported for clearinghouse Maestro transactions for Card Holder not present are not supported for the given clearinghouse
					case 470: return (PaymentStatus.Invalid);
					// 3-D Secure Required This transaction must be a 3dsecure transaction
					case 471: return (PaymentStatus.Invalid);
					// Invalid capturemethod International Maestro is not permitted in a Mail order / telephone order environment.
					case 472: return (PaymentStatus.Invalid);
					// Invalid transaction type Keyed International Maestro transaction not permitted
					case 473: return (PaymentStatus.Invalid);
					// Invalid value for merchantid The Merchant Id provided is invalid
					case 480: return (PaymentStatus.Invalid);
					// Element merchantid required The merchant is expected to provide a Merchant Id with each transaction
					case 481: return (PaymentStatus.Invalid);
					// Invalid element merchantid The merchant is not set to provide Merchant Id for a transaction
					case 482: return (PaymentStatus.Invalid);
					// GE Capital: Inappropriate GE Capital card number The merchant attempted to use a GE Capital card with a BIN that does not belong to them
					case 510: return (PaymentStatus.Invalid);
					#endregion
				}
			}
			return (PaymentStatus.Undefined);
		}
	}
}