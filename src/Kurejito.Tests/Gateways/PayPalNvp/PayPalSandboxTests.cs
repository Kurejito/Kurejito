using Kurejito.Gateways.PayPal;
using Kurejito.Gateways.PayPal.DirectPayment;
using Kurejito.Payments;
using Kurejito.Transport;
using Moq;
using Should;
using Xunit;

namespace Kurejito.Tests.Gateways.PayPalNvp
{
    public class PayPalSandboxTests
    {
        [Fact]
        public void Purchase_Successful_Against_Sandbox()
        {
            var payPalNvpPaymentGateway = new PayPalDirectPaymentGateway(new HttpTransport(), PayPalCredentials.CreateSampleCredentials());
            payPalNvpPaymentGateway.Purchase("REF", 100m, "GBP", new PaymentCard("BEN TAYLOR", "4716034283508634", new CardDate(10,2015),"123",CardType.Visa)).Status.ShouldEqual(PaymentStatus.OK);
        }
    }
}