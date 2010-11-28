using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Kurejito.Transport;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways.SagePay {
	public class SagePayResponseStatusTests : SagePayTestBase {

		private void Verify_Sagepay_Status_Code(string sagePayStatus, PaymentStatus expectedStatus) {
			var http = new Mock<IHttpPostTransport>();
			http
				.Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
				.Returns(MakePostResponse(sagePayStatus));

			var gw = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
			var card = new PaymentCard("I M LOADED", "123412341234134", "1212", "123", CardType.Visa);
			var response = gw.Purchase("123456", 123.45m, "GBP", card);
			Assert.Equal(expectedStatus, response.Status);
			
		}
		[Fact]
		public void SagePay_OK_Response_Translates_To_Ok_PaymentStatus() {
			this.Verify_Sagepay_Status_Code("OK", PaymentStatus.Ok);
		}

		[Fact]
		public void SagePay_NOTAUTHED_Response_Translates_To_Declined_PaymentStatus() {
			this.Verify_Sagepay_Status_Code("NOTAUTHED", PaymentStatus.Declined);
		}

		[Fact]
		public void SagePay_MALFORMED_Response_Translates_To_Invalid_PaymentStatus() {
			this.Verify_Sagepay_Status_Code("MALFORMED", PaymentStatus.Invalid);
		}

		[Fact]
		public void SagePay_INVALID_Response_Translates_To_Invalid_PaymentStatus() {
			this.Verify_Sagepay_Status_Code("INVALID", PaymentStatus.Invalid);
		}

		[Fact]
		public void SagePay_ERROR_Response_Translates_To_Error_PaymentStatus() {
			this.Verify_Sagepay_Status_Code("ERROR", PaymentStatus.Error);
		}
	}
}
