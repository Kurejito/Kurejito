using Kurejito.Gateway.FakeBank;
using Kurejito.Payment;
using Xunit;
using Kurejito.Gateway;

namespace Kurejito.Tests.Gateway.FakeBank {
	public class PaymentGatewayTests {

		[Fact]
		public void FakeBank_Declines_Payments_Over_Ten_Pounds() {
			var gw = new PaymentGateway();
			var response = gw.Purchase(10.01m, new CreditCard());
			Assert.Equal(PaymentStatus.Declined, response.Status);
		}

		[Fact]
		public void FakeBank_Approves_Payments_Under_Ten_Pounds() {
			var gw = new PaymentGateway();
			var response = gw.Purchase(9.99m, new CreditCard());
			Assert.Equal(PaymentStatus.OK, response.Status);
		}
	}
}
