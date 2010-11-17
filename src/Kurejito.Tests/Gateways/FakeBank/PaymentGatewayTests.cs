using Kurejito.Gateways.FakeBank;
using Kurejito.Payments;
using Xunit;

namespace Kurejito.Tests.Gateways.FakeBank {
	public class PaymentGatewayTests {

		[Fact]
		public void FakeBank_Declines_Payments_Over_Ten_Pounds() {
			var gw = new PaymentGateway();
			var response = gw.Purchase("pay001", 10.01m, "GBP", new BankCard());
			Assert.Equal(PaymentStatus.Declined, response.Status);
		}

		[Fact]
		public void FakeBank_Approves_Payments_Under_Ten_Pounds() {
			var gw = new PaymentGateway();
			var response = gw.Purchase("pay001", 9.99m, "GBP", new BankCard());
			Assert.Equal(PaymentStatus.OK, response.Status);
		}
	}
}
