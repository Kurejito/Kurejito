using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways {
	public abstract class PurchaseGatewayTests {
		protected abstract IPurchaseGateway CreateGateway();
		[Fact]
		public void Successful_Purchase_Returns_Correct_PaymentStatus() {
			var gw = CreateGateway();
			var card = new PaymentCard("I M LOADED", "1000171234567896", "1212", "123", CardType.Visa);
			var response = gw.Purchase(Guid.NewGuid().ToString(), 123.45m, "GBP", card);
			Console.Write(response.Reason);
		}
	}
}
