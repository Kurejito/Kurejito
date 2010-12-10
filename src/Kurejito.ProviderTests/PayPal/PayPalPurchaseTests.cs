using System;
using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Payments;
using Kurejito.Transport;
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

        [Theory(Skip = "Got this working with extenal Excel data, but not a useful test yet.")]
        [ExcelData(@"PayPal\PayPalDirectPaymentApiErrorCodes.xls", "select * from DirectPaymentApiErrorCodes")]
        public void Purchase_Successful_Against_Sandbox(double code, string shortMessage, string longMessage, string correctiveAction, string kurejitoErrorCode) {
            //https://cms.paypal.com/uk/cgi-bin/?cmd=_render-content&content_ID=developer/e_howto_testing_SBTestErrorConditions

            Console.WriteLine(code);
           
//            var payPalNvpPaymentGateway = new PayPalDirectPaymentGateway(new HttpTransport(), PayPalEnvironment.NegativeTestAccountSandboxEnvironment());
//            payPalNvpPaymentGateway.Purchase("REF", 100m, "GBP", new PaymentCard("BEN TAYLOR", "4716034283508634", new CardDate(10, 2015), "123", CardType.Visa)).Status.ShouldEqual(PaymentStatus.Ok);
        }
    }
}