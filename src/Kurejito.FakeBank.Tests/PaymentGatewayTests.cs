using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Payments;
using Kurejito.Gateway;

namespace Kurejito.FakeBank.Tests {
	public class PaymentGatewayTests {

		[Fact]
		public void FakeBank_Declines_Payments_Over_Ten_Pounds() {
			var gw = new FakeBank.PaymentGateway();
			var response = gw.Purchase(10.01m, new CreditCard());
			Assert.Equal(PaymentStatus.Declined, response.Status);
		}

		[Fact]
		public void FakeBank_Approves_Payments_Under_Ten_Pounds() {
			var gw = new FakeBank.PaymentGateway();
			var response = gw.Purchase(9.99m, new CreditCard());
			Assert.Equal(PaymentStatus.OK, response.Status);
		}
	}
}
