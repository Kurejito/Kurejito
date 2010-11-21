using Kurejito.Tests.Gateways;
using Kurejito.Gateways.DataCash;
using Kurejito.Transport;

namespace Kurejito.Tests.Purchase.DataCash {
	public class DataCashPurchaseTests : PurchaseGatewayTests {

		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new DataCashPaymentGateway(http, "99002005", "AAm9YtKK"));
		}
	}
}
