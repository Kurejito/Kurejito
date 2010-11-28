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
	// ReSharper disable InconsistentNaming
	public class DataCashHttpMockTests : DataCashTestBase {

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
				}).Returns(MakeXmlResponse(1, "Success result from HTTP mock"));
			var gw = new DataCashPaymentGateway(http.Object, CLIENT_ID, PASSWORD, GATEWAY_URI);
			gw.Purchase(TestData.MerchantReference, TestData.Amount, TestData.Currency, TestData.Card);
			http.VerifyAll();
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Authentication_Client() {
			Verify_Purchase_Xml_Element("Request/Authentication/client", CLIENT_ID);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Authentication_Password() {
			Verify_Purchase_Xml_Element("Request/Authentication/password", PASSWORD);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_CardNumber() {
			Verify_Purchase_Xml_Element("Request/Transaction/CardTxn/Card/pan", TestData.Card.CardNumber);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_ExpiryDate() {
			Verify_Purchase_Xml_Element("Request/Transaction/CardTxn/Card/expirydate", TestData.Card.ExpiryDate.MM_YY);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_MerchantReference() {
			Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/merchantreference", TestData.MerchantReference);
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Amount() {
			Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/amount", TestData.Amount.ToString("0.00"));
		}

		[Fact]
		public void DataCash_Purchase_XML_Contains_Correct_Currency() {
			Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/amount/@currency", TestData.Currency);
		}
	}
}


