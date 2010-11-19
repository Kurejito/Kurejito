using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurejito.Gateways.PayPalNvp;
using Kurejito.Payments;
using Should;
using Xunit;

namespace Kurejito.Tests.Gateways.PayPalNvp
{
    public class PayPalNvpPaymentGatewayTests
    {
        [Fact(Skip="Just exploring the API.  Not a valid test yet.")]
        public void Api_Exploration_Test()
        {
            var payPalCredentials = new PayPalCredentials("username", "password", "signature");
            var payPalNvpPaymentGateway = new PayPalNvpPaymentGateway(payPalCredentials);
            payPalNvpPaymentGateway.Purchase("REF", 100m, "USD", new PaymentCard()).ShouldNotBeNull();
        }
    }
}
