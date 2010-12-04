using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Kurejito.Transport;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways.SagePay {
	public abstract class SagePayTestBase {
		//protected Mock<IHttpPostTransport> http;
		//protected SagePayPaymentGateway gateway;
		protected const string VENDOR_NAME = "rockshop";
		protected const decimal VPS_PROTOCOL = 2.23m;

		private const string POST_RESPONSE_FORMAT = @"VPSProtocol=2.23
Status={0}
StatusDetail=Simulated result from HTTP transport mock
VPSTxId={{00000000-1111-2222-3333-444455556666}}
SecurityKey=1234567890
TxAuthNo=1234
AVSCV2=NO DATA MATCHES
AddressResult=NOTMATCHED
PostCodeResult=NOTMATCHED
CV2Result=NOTMATCHED
";

		protected Mock<IHttpPostTransport> http;

		protected SagePayPaymentGateway gateway;
		protected PaymentCard card;

		protected SagePayTestBase() {
			http = new Mock<IHttpPostTransport>();
			gateway = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
			card = new PaymentCard("I M LOADED", "13412341341234", "1212", "123", CardType.Visa);
		}


		protected string MakePostResponse(string status) {
			return (String.Format(POST_RESPONSE_FORMAT, status));
		}
	}
}
