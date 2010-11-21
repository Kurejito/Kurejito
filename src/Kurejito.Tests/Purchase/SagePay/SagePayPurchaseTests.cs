using Kurejito.Tests.Gateways;
using Kurejito.Transport;
using Kurejito.Gateways.SagePay.VspDirect;

namespace Kurejito.Tests.Purchase.SagePay {
	public class SagePaySimulatorPurchaseTests : PurchaseGatewayTests {
		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new SagePayPaymentGateway(http, "kurejito", 2.23m, GatewayMode.Simulator));
		}
	}
}
