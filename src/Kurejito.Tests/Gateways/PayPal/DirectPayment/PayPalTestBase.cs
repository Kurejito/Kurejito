using System;
using System.Web;
using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Payments;
using Kurejito.Tests.Payments;
using Kurejito.Transport;
using Moq;

namespace Kurejito.Tests.Gateways.PayPal.DirectPayment {
    public class PayPalTestBase {
        public decimal Amount { get;  set; }

        public PayPalTestBase() {
            this.MerchantReference = Guid.NewGuid().ToString();
            this.Environment = PayPalEnvironment.NegativeTestAccountSandboxEnvironmentForGbr();
            this.InitWithResponse(FakePayPalResponse.Success.ToString());
            this.Amount = 100m;
        }

        protected void InitWithResponse(string response) {
            this.HttpTransportMock = new Mock<IHttpPostTransport>();
            this.HttpTransportMock.Setup(t => t.Post(It.IsAny<Uri>(), It.IsAny<string>())).Returns(response);
            this.Gateway = new PayPalDirectPaymentGateway(this.HttpTransportMock.Object, this.Environment);
        }

        protected PayPalEnvironment Environment { get; set; }

        protected PayPalDirectPaymentGateway Gateway { get; set; }

        protected Mock<IHttpPostTransport> HttpTransportMock { get; set; }

        protected PaymentResponse DoValidPurchaseRequest() {
            return Gateway.Purchase(MerchantReference, new Money(this.Amount, new Currency("GBP")), TestPaymentCards.VisaValid);
        }

        protected string MerchantReference { get; private set; }

        protected void VerifyRequestPair(string key, string value) {
            this.HttpTransportMock.Verify(t => t.Post(It.IsAny<Uri>(),It.Is<string>(s => HttpUtility.ParseQueryString(s)[key].Equals(value))));
        }
    }
}