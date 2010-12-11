using System;
using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Payments;
using Kurejito.Transport;
using Should;
using Xunit.Extensions;

namespace Kurejito.ProviderTests.PayPal {
    public class PayPalPurchaseTests : PurchaseGatewayTests {
        protected override IPurchaseGateway CreateGateway() {
            var http = new HttpTransport();
            return (new PayPalDirectPaymentGateway(new HttpTransport(), PayPalEnvironment.NegativeTestAccountSandboxEnvironment()));
        }

        protected override PaymentCard GetMagicCard(PaymentStatus status) {
            throw new NotImplementedException();
        }

        [Theory]
        [ExcelData(@"PayPal\PayPalDirectPaymentApiErrorCodes.xls", "select * from DirectPaymentApiErrorCodes")]
        public void PayPal_Error_Code_CorrectlyTranslates_To_Kurejito_Code(double code, string shortMessage, string longMessage, string correctiveAction, string kurejitoErrorCode) {
            //https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_testing_SBTestErrorConditions

            var payPalNvpPaymentGateway = new PayPalDirectPaymentGateway(new HttpTransport(), PayPalEnvironment.NegativeTestAccountSandboxEnvironment());
            var amount = Convert.ToDecimal(code/100);
            var expectedStatus = Enum.Parse(typeof (PaymentStatus), kurejitoErrorCode);
            var paymentCard = new PaymentCard("BEN TAYLOR", "4716034283508634", new CardDate(10, 2015), "123", CardType.Visa);
            payPalNvpPaymentGateway.Purchase("REF", amount, "GBP", paymentCard).Status.ShouldEqual(expectedStatus);
        }
    }
}