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
	public class SagePaySimulatorTests : SagePayTestBase {
		
		public SagePaySimulatorTests() {
			http = new Mock<IHttpPostTransport>();
			gateway = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
			card = new PaymentCard("I M LOADED", "13412341341234", "1212", "123", CardType.Visa);
		}

		//[Fact]
		//public void Verify_Response_From_SagePay_Simulator() {

		//    var gw = new SagePayPaymentGateway(new HttpTransport(), "spotlight", 2.23m, GatewayMode.Simulator);

		//    var response = gw.Purchase("123456", 123.45m, "GBP", card);
		//    Console.Write(response.Reason);
		//}
	}
}
