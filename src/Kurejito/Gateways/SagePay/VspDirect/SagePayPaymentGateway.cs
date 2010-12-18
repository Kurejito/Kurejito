using System;
using System.Collections.Generic;
using System.Linq;
using Kurejito.Payments;
using Kurejito.Transport;
using System.Web;

namespace Kurejito.Gateways.SagePay.VspDirect {
	/// <summary>A payment gateway interface to the SagePay VspDirect payment system.</summary>
	public class SagePayPaymentGateway : IPurchaseGateway {
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
		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
			var lineItem = new LineItem(String.Format("Transaction ref {0}", merchantReference), amount);
			var basket = new Basket(lineItem);
			return (this.Purchase(merchantReference, amount, currency, card, basket));
		}


		/// <summary>Attempt to take the specified payment amount from the supplied payment card.</summary>
		/// <param name="merchantReference">The merchant reference.</param>
		/// <param name="amount">The amount.</param>
		/// <param name="currency">The currency.</param>
		/// <param name="card">The card.</param>
		/// <param name="basket">The basket.</param>
		/// <returns></returns>
		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card, Basket basket) {
			var data = MakePostData();
			data.Add("TxType", "PAYMENT");
			data.Add("VendorTxCode", merchantReference);
			data.Add("Amount", amount.ToString("0.00"));
			data.Add("Currency", currency);
			data.Add("CardHolder", card.CardHolder);
			data.Add("CardNumber", card.CardNumber);
			data.Add("CardType", TranslateCardType(card.CardType));
			data.Add("ExpiryDate", card.ExpiryDate.ToString());
			data.Add("Basket", basket.ToSagePayBasketFormat());
			data.Add("Description", "DUMMY DESCRIPTION");
			var postData = FormatPostData(data);
			var uri = postUris[this.mode];
			var httpResponse = http.Post(uri, postData);
			var status = this.ExtractStatus(httpResponse);
			return (new PaymentResponse() {
			                              	Status = status,
			                              	Reason = httpResponse
			                              });
		}

		private PaymentStatus ExtractStatus (string postResponse) {
			var pairs = postResponse.Split('\r', '\n');
			foreach(var pair in pairs) {
				var tokens = pair.Split(new char[] {'='}, 2);
				if (tokens.Length == 2 && tokens[0] == "Status") {
					switch(tokens[1].ToUpper()) {
						case "OK":
							return (PaymentStatus.Ok);
						case "MALFORMED":
						case "INVALID":
							return (PaymentStatus.Invalid);
						case "NOTAUTHED":
							return (PaymentStatus.Declined);
					}
				}
			}
			return (PaymentStatus.Error);
		}
	}
}
