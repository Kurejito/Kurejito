using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Kurejito.Transport;
using System.Xml.Linq;
using System.Xml.XPath;
using Kurejito.Gateways.DataCash;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways.DataCash {
	public class DataCashHttpMockTests {
		private string clientId = "99002005";
		private string password = "AAm9YtKK";
		private static class Purchase {
			public static string MerchantReference = Guid.NewGuid().ToString();
			public static decimal Amount = 123.45m;
			public static string Currency = "GBP";
			public static PaymentCard Card = new PaymentCard("I M LOADED", "1234123413241234", "1212", "123", CardType.Visa);
		}

		private void Verify_Purchase_Xml_Element(string xpath, string expectedValue) {
			var http = new Mock<IHttpPostTransport>();
			http
				.Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
				.Callback((Uri uri, string postData) => {
					var xml = XDocument.Parse(postData);
					var element = xml.XPathEvaluate(xpath);
					Assert.NotNull(element);
					if (element is XElement) {
						Assert.Equal(expectedValue, ((XElement)element).Value);
					} else if (element is XAttribute) {
						Assert.Equal(expectedValue, ((XAttribute)element).Value);
					}
				});
			var gw = new DataCashPaymentGateway(http.Object, clientId, password);
			gw.Purchase(Purchase.MerchantReference, Purchase.Amount, Purchase.Currency, Purchase.Card);
			http.VerifyAll();
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Authentication_Client() {
			Verify_Purchase_Xml_Element("Request/Authentication/client", clientId);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Authentication_Password() {
			Verify_Purchase_Xml_Element("Request/Authentication/password", password);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_CardNumber() {
			Verify_Purchase_Xml_Element("Request/Transaction/CardTxn/Card/pan", Purchase.Card.CardNumber);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_ExpiryDate() {
			Verify_Purchase_Xml_Element("Request/Transaction/CardTxn/Card/expirydate", Purchase.Card.ExpiryDate.MM_YY);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_MerchantReference() {
			Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/merchantreference", Purchase.MerchantReference);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Amount() {
			Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/amount", Purchase.Amount.ToString("0.00"));
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Currency() {
			Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/amount/@currency", Purchase.Currency);
		}
	}
}
