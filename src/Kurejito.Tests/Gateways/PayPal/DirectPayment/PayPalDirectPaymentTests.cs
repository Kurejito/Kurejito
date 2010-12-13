using System;
using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Payments;
using Kurejito.Transport;
using Moq;
using Should;
using Xunit;
using Xunit.Extensions;

namespace Kurejito.Tests.Gateways.PayPal.DirectPayment {
    public class PayPalDirectPaymentTests {
        private const string ACK_FAILURE_RESPONSE = @"TIMESTAMP=2010%2d12%2d12T16%3a15%3a11Z&CORRELATIONID=a9adbf79d0949&ACK={{ACK}}&VERSION=56%2e0&BUILD=1620725&L_ERRORCODE0={{ERROR_CODE}}&L_SHORTMESSAGE0={{SHORT_MESSAGE}}&L_LONGMESSAGE0=This%20transaction%20cannot%20be%20processed%2e&L_SEVERITYCODE0=Error&L_ERRORPARAMID0=ProcessorResponse&L_ERRORPARAMVALUE0=0000&AMT=105%2e05&CURRENCYCODE=GBP&AVSCODE=X&CVV2MATCH=S";
        private const string ACK_SUCCESS_RESPONSE = @"TIMESTAMP=2010%2d12%2d12T16%3a12%3a00Z&CORRELATIONID=9809a922ec57b&ACK=Success&VERSION=56%2e0&BUILD=1603674&AMT=100%2e00&CURRENCYCODE=GBP&AVSCODE=X&CVV2MATCH=S&TRANSACTIONID=58840544LM668971C";

        [Theory(Skip = "Not implemented the code for this yet.")]
        [ExcelData(@"PayPal\PayPalDirectPaymentApiErrorCodes.xls", "select * from DirectPaymentApiErrorCodes")]
        public void PayPal_Error_Code_CorrectlyTranslates_To_Kurejito_Code(double code, string kurejitoErrorCode, string shortMessage, string longMessage, string correctiveAction) {
            //https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_testing_SBTestErrorConditions

            var payPalNvpPaymentGateway = new PayPalDirectPaymentGateway(new HttpTransport(), PayPalEnvironment.NegativeTestAccountSandboxEnvironment());
            var amount = Convert.ToDecimal(code/100);
            var expectedStatus = Enum.Parse(typeof (PaymentStatus), kurejitoErrorCode);
            var paymentCard = new PaymentCard("BEN TAYLOR", "4716034283508634", new CardDate(10, 2015), "123", CardType.Visa);
            payPalNvpPaymentGateway.Purchase("REF", amount, "GBP", paymentCard).Status.ShouldEqual(expectedStatus);
        }

        [Theory]
        [InlineData("10505")]
        [InlineData("10546")]
        [InlineData("11821")]
        [InlineData("15002")]
        [InlineData("10553")]
        public void Purchase_With_Gateway_Decline_Response_Should_Return_Declined(string errorCode) {
            DoPurchaseWithErrorResponse(errorCode, "Gateway%20Decline", "Failure").Status.ShouldEqual(PaymentStatus.Declined);
        }

        [Fact]
        public void Purchase_With_Partial_Success_Response_Should_Throw_NotSupportedException_As_Only_For_Parrallel_Payments() {
            Assert.Throws(typeof(NotSupportedException), () => DoPurchaseWithErrorResponse("00000", "Irrelevant", "PartialSuccess").Status.ShouldEqual(PaymentStatus.Declined));
        }

        private static PaymentResponse DoPurchaseWithErrorResponse(string errorCode, string errorShortMessage, string ackValue) {
            if (errorCode == null) throw new ArgumentNullException("errorCode");
            return DoPurchaseWithMockTransport(CreateMockTransportWithErrorResponse(errorCode, errorShortMessage, ackValue));
        }

        private static PaymentResponse DoPurchaseWithMockTransport(Mock<IHttpPostTransport> transportMock) {
            var payPalNvpPaymentGateway = new PayPalDirectPaymentGateway(transportMock.Object, PayPalEnvironment.NegativeTestAccountSandboxEnvironment());
            var paymentCard = new PaymentCard("BEN TAYLOR", "4716034283508634", new CardDate(10, 2015), "123", CardType.Visa);
            return payPalNvpPaymentGateway.Purchase("REF", 100m, "GBP", paymentCard);
        }

        private static Mock<IHttpPostTransport> CreateMockTransportWithErrorResponse(string errorCode, string errorShortMessage, string ackValue)
        {
            var transportMock = new Mock<IHttpPostTransport>();
            var response = ACK_FAILURE_RESPONSE.Replace("{{ERROR_CODE}}", errorCode).Replace("{{SHORT_MESSAGE}}", errorShortMessage).Replace("{{ACK}}", ackValue);
            transportMock.Setup(tm => tm.Post(It.IsAny<Uri>(), It.IsAny<string>())).Returns(response);
            return transportMock;
        }
    }
}