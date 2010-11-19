using System;
using System.Net;
using System.Runtime.Serialization;
using Kurejito.Gateways.PayPalNvp;
using Kurejito.Payments;
using Moq;
using Should;
using Xunit;

namespace Kurejito.Tests.Gateways.PayPalNvp
{
    public class PayPalNvpPaymentGatewayTests
    {
        [Fact]
        public void Api_Exploration_Test()
        {
            var payPalNvpPaymentGateway = new PayPalNvpPaymentGateway(PayPalCredentials.CreateSampleCredentials()); 
            payPalNvpPaymentGateway.Purchase("REF", 100m, "USD", new PaymentCard()).ShouldNotBeNull();
        }
    }
}