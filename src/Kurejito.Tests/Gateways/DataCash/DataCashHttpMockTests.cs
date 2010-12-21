using System;
using System.Xml.Linq;
using System.Xml.XPath;
using Kurejito.Gateways.DataCash;
using Kurejito.Transport;
using Moq;
using Xunit;

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
                                  Assert.Equal(expectedValue, ((XElement) element).Value);
                              } else if (element is XAttribute) {
                                  Assert.Equal(expectedValue, ((XAttribute) element).Value);
                              }
                          }).Returns(this.MakeXmlResponse(1, "Success result from HTTP mock"));
            var gw = new DataCashPaymentGateway(http.Object, CLIENT_ID, PASSWORD, GATEWAY_URI);
            gw.Purchase(TestData.MerchantReference, TestData.Amount, TestData.Card);
            http.VerifyAll();
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_Authentication_Client() {
            this.Verify_Purchase_Xml_Element("Request/Authentication/client", CLIENT_ID);
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_Authentication_Password() {
            this.Verify_Purchase_Xml_Element("Request/Authentication/password", PASSWORD);
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_CardNumber() {
            this.Verify_Purchase_Xml_Element("Request/Transaction/CardTxn/Card/pan", TestData.Card.CardNumber);
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_ExpiryDate() {
            this.Verify_Purchase_Xml_Element("Request/Transaction/CardTxn/Card/expirydate", TestData.Card.ExpiryDate.MM_YY);
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_MerchantReference() {
            this.Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/merchantreference", TestData.MerchantReference);
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_Amount() {
            this.Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/amount", TestData.Amount.ToString("0.00"));
        }

        [Fact]
        public void DataCash_Purchase_XML_Contains_Correct_Currency() {
            this.Verify_Purchase_Xml_Element("Request/Transaction/TxnDetails/amount/@currency", TestData.Amount.Currency.Iso3LetterCode);
        }
    }
}