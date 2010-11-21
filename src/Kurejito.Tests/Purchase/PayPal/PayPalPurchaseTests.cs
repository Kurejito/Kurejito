using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Transport;

namespace Kurejito.Tests.Purchase.PayPal {
	public class PayPalPurchaseTests : PurchaseGatewayTests {
		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new PayPalDirectPaymentGateway(new HttpTransport(), PayPalCredentials.CreateSampleCredentials()));
		}
		public override void Purchase_With_Invalid_Card_Number_Returns_Correct_PaymentStatus() {
			base.Purchase_With_Invalid_Card_Number_Returns_Correct_PaymentStatus();
		}
		public override void Successful_Purchase_Returns_Correct_PaymentStatus() {
			base.Successful_Purchase_Returns_Correct_PaymentStatus();
		}
	}
}
