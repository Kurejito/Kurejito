using System;
using System.Web;
using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Tests.Payments;
using Kurejito.Transport;
using Moq;

namespace Kurejito.Tests.Gateways.PayPal.DirectPayment {
    public class PayPalTestBase {
        public PayPalTestBase() {
            this.MerchantReference = Guid.NewGuid().ToString();
            this.Environment = PayPalEnvironment.NegativeTestAccountSandboxEnvironment();
            this.InitWithResponse(FakePayPalResponse.Success.ToString());
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
            return Gateway.Purchase(MerchantReference, 100m, "GBP", TestPaymentCards.VisaValid);
        }

        protected string MerchantReference { get; private set; }

        protected void VerifyRequestPair(string key, string value) {
            this.HttpTransportMock.Verify(t => t.Post(It.IsAny<Uri>(),It.Is<string>(s => HttpUtility.ParseQueryString(s)[key].Equals(value))));
        }
    }
}