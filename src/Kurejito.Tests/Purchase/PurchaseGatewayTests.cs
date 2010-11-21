using System;
using Xunit;
using Kurejito.Payments;
using System.Diagnostics;

namespace Kurejito.Tests.Purchase {
	public abstract class PurchaseGatewayTests {
		protected abstract IPurchaseGateway CreateGateway();
		[Fact]
		public virtual void Successful_Purchase_Returns_Correct_PaymentStatus() {
			var gw = CreateGateway();
			var card = new PaymentCard("I M LOADED", "4716034283508634", "1212", "123", CardType.Visa);
			var response = gw.Purchase(Guid.NewGuid().ToString(), 123.45m, "GBP", card);
			Debug.WriteLine(response.Reason);
			Assert.Equal(PaymentStatus.OK, response.Status);
		}

		[Fact]
		public virtual void Purchase_With_Invalid_Card_Number_Returns_Correct_PaymentStatus() {
			var gw = CreateGateway();
			var card = new PaymentCard("I M LOADED", "foo", "1212", "123", CardType.Visa);
			var response = gw.Purchase(Guid.NewGuid().ToString(), 123.45m, "GBP", card);
			Debug.WriteLine(response.Reason);
			Assert.Equal(PaymentStatus.OK, response.Status);
		}

	}
}
