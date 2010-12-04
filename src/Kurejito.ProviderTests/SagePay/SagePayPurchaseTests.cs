using System;
using Kurejito.Payments;
using Kurejito.Tests.Gateways;
using Kurejito.Transport;
using Kurejito.Gateways.SagePay.VspDirect;

namespace Kurejito.ProviderTests.SagePay {
	public class SagePaySimulatorPurchaseTests : PurchaseGatewayTests {
		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			throw (new Exception("You can't run these tests without a live SagePay test account - sorry."));
//			return (new SagePayPaymentGateway(http, "myVendor", 2.23m, GatewayMode.Simulator));
		}

		protected override Payments.PaymentCard GetMagicCard(PaymentStatus status) {
			switch(status) {
				case PaymentStatus.Ok:
					return (new PaymentCard("I M LOADED", "4929000000006", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Declined:
					return (new PaymentCard("I M BUSTED", "4408041234567893", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Invalid:
					return (new PaymentCard("1234123412341324", "I M STUPID", EXPIRY_DATE, "123", CardType.Visa));
			}
			throw new ArgumentOutOfRangeException("status");
		}
	}
}
