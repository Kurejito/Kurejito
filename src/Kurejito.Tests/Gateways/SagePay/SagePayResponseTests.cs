using System;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;
using Kurejito.Transport;
using Moq;
using Xunit;

namespace Kurejito.Tests.Gateways.SagePay {
    public class SagePayResponseTests : SagePayTestBase {
        private void Verify_Sagepay_Status_Code(string sagePayStatus, PaymentStatus expectedStatus) {
            var http = new Mock<IHttpPostTransport>();
            http
                .Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(this.MakePostResponse(sagePayStatus));

            var gw = new SagePayPaymentGateway(http.Object, VENDOR_NAME, VPS_PROTOCOL, GatewayMode.Simulator);
            var card = new PaymentCard("I M LOADED", "123412341234134", "1212", "123", CardType.Visa);
            var response = gw.Purchase("123456", new Money(123.45m, new Currency("GBP")), card);
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

        [Fact]
        public void SagePay_Response_Contains_Payment_Id() {
            var http = new Mock<IHttpPostTransport>();
            var vpsTxId = "{00001111-2222-3333-4444-556677889900}";
            var sagepay = new SagePayPaymentGateway(http.Object, "rockshop", 2.23m, GatewayMode.Live);
            http
                .Setup(h => h.Post(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(this.MakePostResponse("OK", vpsTxId));
            var card = new PaymentCard("I M LOADED", "123412341234134", "1212", "123", CardType.Visa);
            var response = sagepay.Purchase("1234", new Money(123.45m, new Currency("GBP")), card);
            Assert.Equal(vpsTxId, response.PaymentId);
        }
    }
}