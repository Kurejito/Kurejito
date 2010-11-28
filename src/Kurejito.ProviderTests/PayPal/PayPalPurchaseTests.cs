using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Transport;

namespace Kurejito.ProviderTests.PayPal {
	public class PayPalPurchaseTests : PurchaseGatewayTests {
		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new PayPalDirectPaymentGateway(new HttpTransport(), PayPalCredentials.CreateSampleCredentials()));
		}

		protected override Payments.PaymentCard GetMagicCard(PaymentStatus status) {
			throw new System.NotImplementedException();
		}
	}
}
