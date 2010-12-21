using System;
using System.Collections.Generic;
using System.Linq;
using Kurejito.Payments;
using Kurejito.Transport;
using System.Web;

namespace Kurejito.Gateways.SagePay.VspDirect {
	/// <summary>A payment gateway interface to the SagePay VspDirect payment system.</summary>
	public class SagePayPaymentGateway : IPurchase {
		private readonly string vendorName;
		private readonly decimal vpsProtocol;
		private readonly GatewayMode mode;
		private readonly IHttpPostTransport http;

		private static readonly Dictionary<GatewayMode, Uri> postUris = new Dictionary<GatewayMode, Uri>() {
			{ GatewayMode.Simulator, new Uri("https://test.sagepay.com/Simulator/VSPDirectGateway.asp") },
			{ GatewayMode.Test, new Uri("https://test.sagepay.com/gateway/service/vspdirect-register.vsp") },
			{ GatewayMode.Live, new Uri("https://live.sagepay.com/gateway/service/vspdirect-register.vsp") },
		};

		/// <summary>Construct a new <see cref="SagePayPaymentGateway" /> that uses the supplied <see cref="IHttpPostTransport" /> to communicate with the remote server.</summary>
		/// <param name="http">An instance of a class implementing <see cref="IHttpPostTransport"/> to provide HTTP POST capabilities.</param>
		/// <param name="vendorName">The vendor name used by this SagePay installation.</param>
		/// <param name="vpsProtocol">The version of the VPS Protocol used by the remote SagePay system.</param>
		/// <param name="mode"></param>
		public SagePayPaymentGateway(IHttpPostTransport http, string vendorName, decimal vpsProtocol, GatewayMode mode) {
			this.http = http;
			this.mode = mode;
			this.vendorName = vendorName;
			this.vpsProtocol = vpsProtocol;
		}

		private IDictionary<string, string> MakePostData() {
			IDictionary<string, string> data = new Dictionary<string, string>
			{
			    {"Vendor", vendorName},
			    {"VPSProtocol", vpsProtocol.ToString()}
			};
			return (data);
		}

		private string FormatPostData(IDictionary<string, string> data) {
			var values = data.Select(pair => String.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
			return (String.Join("&", values.ToArray()));
		}

		private string TranslateCardType(CardType cardType) {
			switch (cardType) {
				case CardType.MasterCard: return ("MC");
				case CardType.Visa: return ("VISA");
			}
			throw (new NotImplementedException("Only Visa and Mastercard currently supported!"));
		}

		/// <summary>Attempts to debit the specified amount from the supplied payment card.</summary>
		/// <remarks>Because the SagePay gateway requires a shopping basket, this overload will create
		///  a simple basket containing a single line item whose description is auto-generated from the 
		/// supplied order details.</remarks>
		public PaymentResponse Purchase(string merchantReference, Money amount, PaymentCard card) {
            var data = MakePostData();
            data.Add("TxType", "PAYMENT");
            data.Add("VendorTxCode", merchantReference);
            data.Add("Amount", amount.ToString("0.00"));
            data.Add("Currency", amount.Currency.Iso3LetterCode);
            data.Add("CardHolder", card.CardHolder);
            data.Add("CardNumber", card.CardNumber);
            data.Add("CardType", TranslateCardType(card.CardType));
            data.Add("ExpiryDate", card.ExpiryDate.ToString());
		    data.Add("Basket", CreateBasketString(merchantReference, amount));
            data.Add("Description", "DUMMY DESCRIPTION");
            var postData = FormatPostData(data);
            var uri = postUris[this.mode];
            var httpResponse = http.Post(uri, postData);
            var response = this.ParseResponse(httpResponse);
            return (response);
        }

	    private static string CreateBasketString(string merchantReference, decimal amount) {
	        var desc = String.Format("Transaction ref {0}", merchantReference).ToASCIIString();
	        var token = String.Format("{0}:{1}:{2}:{3}:{4}:{5}", desc, "", "", "", "", amount.ToString("0.00"));
	        var tokens = new List<string> {"1", token};
	        return String.Join(":", tokens.ToArray());
	    }

	    private static IEnumerable<string[]> Tokenize(string postResponse) {
			var possiblePairs = postResponse.Split('\r', '\n');
			var verifiedPairs = possiblePairs.Where(pair => (pair != null && pair.Contains("=")));
			var returnedPairs = verifiedPairs.Select(pair => pair.Split(new char[] {'='}, 2));
			return (returnedPairs);
		}

		private PaymentResponse ParseResponse(string postResponse) {
			var response = new PaymentResponse {
				Reason = postResponse
			};
			foreach (var pair in Tokenize(postResponse)) {
				this.PopulateResponseProperty(response, pair);
			}
			return (response);
		}

		private void PopulateResponseProperty(PaymentResponse response, string[] pair) {
			if (pair.Length != 2) throw (new ArgumentException("Token " + string.Join(",", pair) + " was not an array with exactly two elements", "pair"));
			switch (pair[0]) {
				case "Status":
					response.Status = ExtractStatus(pair);
					return;
				case "VPSTxId":
					response.PaymentId = pair[1];
					return;
			}
		}

		private PaymentStatus ExtractStatus(string[] pair) {
			switch (pair[1].ToUpper()) {
				case "OK":
					return (PaymentStatus.Ok);
				case "MALFORMED":
				case "INVALID":
					return (PaymentStatus.Invalid);
				case "NOTAUTHED":
					return (PaymentStatus.Declined);
                case "ERROR"://TODO DB review this as added in by BT to fix red test.
                    return PaymentStatus.Error;
			}
			return (PaymentStatus.Undefined);
		}
	}
}
