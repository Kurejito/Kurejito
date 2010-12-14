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
    public class PayPalDirectPaymentTests : PayPalTestBase {
        [Fact]
        public void Post_Should_Contain_Version_Number_From_Environment() {
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("VERSION", this.Environment.Version);
        }

        [Fact]
        public void Post_Should_Contain_Signature_From_Environment() {
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("SIGNATURE", this.Environment.Signature);
        }

        [Fact]
        public void Post_Should_Contain_Username_From_Environment() {
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("USER", this.Environment.Username);
        }

        [Fact]
        public void Post_Should_Contain_Password_From_Environment() {
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("PWD", this.Environment.Password);
        }

        [Fact]
        public void Post_Uri_Should_Match_Environment_Uri() {
            this.DoValidPurchaseRequest();
            this.HttpTransportMock.Verify(t => t.Post(It.Is<Uri>(uri => uri.Equals(this.Environment.Uri)), It.IsAny<string>()));
        }

        [Theory]
        [InlineData("10505")]
        [InlineData("10546")]
        [InlineData("11821")]
        [InlineData("15002")]
        [InlineData("10553")]
        public void Purchase_With_Gateway_Decline_Response_Should_Return_Declined(string errorCode) {
            this.InitWithResponse(FakePayPalResponse.AckFailure().WithError("Gateway Decline", errorCode));
            this.DoValidPurchaseRequest().Status.ShouldEqual(PaymentStatus.Declined);
        }

        [Fact]
        public void Purchase_Request_Should_Use_Method_Do_Direct_Payment() {
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("METHOD", "DoDirectPayment");
        }

        [Theory]
        [InlineData(100.20)]
        [InlineData(22.99)]
        [InlineData(100)]
        [InlineData(9999.99)]
        [InlineData(.01)]
        public void Purchase_Request_Should_Have_PayPal_Formatted_Amount(double amount)
        {
            //TODO currently amount could have more than two decimal places and we would probably lose that which is bad.  Get Money back in.
            this.Amount = Convert.ToDecimal(amount);
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("AMT", this.Amount.ToString("0.00"));
        }
        
        [Fact]
        public void Purchase_Request_Should_Use_PaymentAction_Sale()
        {
            this.DoValidPurchaseRequest();
            this.VerifyRequestPair("PAYMENTACTION", "Sale");
        }

        [Fact]
        public void Purchase_With_Partial_Success_Response_Should_Throw_NotSupportedException_As_Only_For_Parrallel_Payments() {
            this.InitWithResponse(FakePayPalResponse.AckPartialSuccess().WithError("Gateway Decline", "00000"));
            Assert.Throws(typeof (NotSupportedException), () => this.DoValidPurchaseRequest().Status.ShouldEqual(PaymentStatus.Declined));
        }

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
    }
}