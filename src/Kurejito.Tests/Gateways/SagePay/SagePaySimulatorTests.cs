using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Kurejito.Transport;
using Moq;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;
using System.Web;
using System.Collections.Specialized;

namespace Kurejito.Tests.Gateways.SagePay {
	public class SagePaySimulatorPurchaseTests : PurchaseGatewayTests, IPurchaseGatewayTests {
		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new SagePayPaymentGateway(http, "kurejito", 2.23m, GatewayMode.Simulator));
		}
	}
}
