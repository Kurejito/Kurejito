using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;
using Kurejito.Transport;
using Moq;
using Xunit;
using System.Web;

namespace Kurejito.Tests.Gateways.SagePay {
	public class SagePayBasketTests : SagePayTestBase {

		public SagePayBasketTests() {
			http = new Mock<IHttpPostTransport>();
			gateway = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
			card = new PaymentCard("I M LOADED", "13412341341234", "1212", "123", CardType.Visa);
		}

		[Fact]
		public void SagePay_Post_With_No_Basket_Creates_Simple_Basket() {
			http.Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
				.Callback((Uri uri, string post) => {
					var values = HttpUtility.ParseQueryString(post);
				    Assert.Equal("1:Transaction ref 1234:::::123.45", values["basket"]);
				})
				.Returns(MakePostResponse("OK"));
			var sagePay = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
			sagePay.Purchase("1234", 123.45m, "GBP", card);
			http.VerifyAll();
		}
	}
}
