using Kurejito.Gateways.FakeBank;
using Kurejito.Payments;
using Xunit;

namespace Kurejito.Tests.Gateways.FakeBank {
	public class PaymentGatewayTests {

		[Fact]
		public void FakeBank_Declines_Payments_Over_Ten_Pounds() {
			var gw = new PaymentGateway();
			var response = gw.Purchase("pay001", new Money(10.01m, new Currency("GBP")), new PaymentCard());
			Assert.Equal(PaymentStatus.Declined, response.Status);
		}

		[Fact]
		public void FakeBank_Approves_Payments_Under_Ten_Pounds() {
			var gw = new PaymentGateway();
			var response = gw.Purchase("pay001", new Money(9.99m, new Currency("GBP")), new PaymentCard());
			Assert.Equal(PaymentStatus.Ok, response.Status);
		}
	}
}
