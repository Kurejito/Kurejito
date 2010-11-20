using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Gateways.DataCash;
using Kurejito.Transport;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways.DataCash {
	public class DataCashPurchaseTests : PurchaseGatewayTests {

		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new DataCashPaymentGateway(http, "99002005", "AAm9YtKK"));
		}
	}
}
