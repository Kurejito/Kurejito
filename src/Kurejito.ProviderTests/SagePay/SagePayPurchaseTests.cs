using System;
using Kurejito.Payments;
using Kurejito.Tests.Gateways;
using Kurejito.Transport;
using Kurejito.Gateways.SagePay.VspDirect;

namespace Kurejito.ProviderTests.SagePay {
	public class SagePaySimulatorPurchaseTests : PurchaseGatewayTests {
		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new SagePayPaymentGateway(http, "kurejito", 2.23m, GatewayMode.Simulator));
		}

		protected override Payments.PaymentCard GetMagicCard(PaymentStatus status) {
			switch(status) {
				case PaymentStatus.Ok:
					return (new PaymentCard("I M LOADED", "4929000000006", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Declined:
					return (new PaymentCard("I M BROKE", "4929123412341234", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Invalid:
					return (new PaymentCard("1234123412341324", "I M STUPID", EXPIRY_DATE, "123", CardType.Visa));
			}
			throw new ArgumentOutOfRangeException("status");
		}
	}
}
