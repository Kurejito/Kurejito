using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurejito.Payments;
using Kurejito.Transport;
using System.Web;

namespace Kurejito.Gateways.SagePay.VspDirect {
	/// <summary>A payment gateway interface to the SagePay VspDirect payment system.</summary>
	public class SagePayPaymentGateway : IPurchaseGateway {
		private string vendorName;
		private decimal vpsProtocol;
		private GatewayMode mode;
		private IHttpPostTransport http;

		private static Dictionary<GatewayMode, Uri> postUris = new Dictionary<GatewayMode, Uri>() {
			{ GatewayMode.Simulator, new Uri("https://test.sagepay.com/Simulator/VSPDirectGateway.asp") },
			{ GatewayMode.Test, new Uri("https://test.sagepay.com/gateway/service/vspdirect-register.vsp") },
			{ GatewayMode.Live, new Uri("https://live.sagepay.com/gateway/service/vspdirect-register.vsp") },
		};
		
		/// <summary>Construct a new <see cref="SagePayPaymentGateway" /> that uses the supplied <see cref="IHttpPostTransport" /> to communicate with the remote server.</summary>
		/// <param name="http">An instance of a class implementing <see cref="IHttpPostTransport"/> to provide HTTP POST capabilities.</param>
		/// <param name="vendorName">The vendor name used by this SagePay installation.</param>
		/// <param name="vpsProtocol">The version of the VPS Protocol used by the remote SagePay system.</param>
		public SagePayPaymentGateway(IHttpPostTransport http, string vendorName, decimal vpsProtocol, GatewayMode mode) {
			this.http = http;
			this.mode = mode;
			this.vendorName = vendorName;
			this.vpsProtocol = vpsProtocol;
		}

		private IDictionary<string, string> MakePostData() {
			IDictionary<string, string> data = new Dictionary<string, string>();
			data.Add("Vendor", this.vendorName);
			data.Add("VPSProtocol", this.vpsProtocol.ToString());
			return (data);
		}

		private string FormatPostData(IDictionary<string, string> data) {
			var values = data.Select(pair => String.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
			return (String.Join("&", values.ToArray()));
		}
		private string TranslateCardType(CardType cardType) {
			switch (cardType) {
				case CardType.Mastercard: return ("MC");
				case CardType.Visa: return ("VISA");
			}
			throw (new NotImplementedException("Only Visa and Mastercard currently supported!"));
		}

		/// <summary>Attempts to debit the specified amount from the supplied payment card.</summary>
		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
			var data = MakePostData();
			data.Add("TxType", "PAYMENT");
			data.Add("VendorTxCode", merchantReference);
			data.Add("Amount", amount.ToString("0.00"));
			data.Add("Currency", currency);
			data.Add("CardHolder", card.CardHolder);
			data.Add("CardNumber", card.CardNumber);
			data.Add("CardType", TranslateCardType(card.CardType));
			data.Add("ExpiryDate", card.ExpiryDate.ToString());
			var postData = FormatPostData(data);
			var uri = postUris[this.mode];
			var httpResponse = http.Post(uri, postData);
			return (new PaymentResponse() { Reason = httpResponse });
		}
	}
}
