using System;
using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Payments;
using Kurejito.Transport;
using Xunit;

namespace Kurejito.ProviderTests.PayPal {
    public class PayPalPurchaseTests : PurchaseGatewayTests {
        protected override IPurchaseGateway CreateGateway() {
            var http = new HttpTransport();
            return (new PayPalDirectPaymentGateway(new HttpTransport(), PayPalEnvironment.NegativeTestAccountSandboxEnvironment()));
        }

        protected override PaymentCard GetMagicCard(PaymentStatus status) {
            throw new NotImplementedException();
        }

        [Fact]
        public void Test_Name() {
	        
        }
    }
}