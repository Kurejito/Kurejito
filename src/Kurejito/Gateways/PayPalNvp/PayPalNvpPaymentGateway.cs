using System;
using System.Collections.Specialized;
using System.Net;
using Kurejito.Payments;

namespace Kurejito.Gateways.PayPalNvp
{
    public class PayPalNvpPaymentGateway : IPurchaseGateway
    {
        private readonly PayPalCredentials _payPalCredentials;

        public PayPalNvpPaymentGateway(PayPalCredentials payPalCredentials)
        {
            if (payPalCredentials == null) throw new ArgumentNullException("payPalCredentials");
            _payPalCredentials = payPalCredentials;
        }

        public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card)
        {
            //PayPal spec I am playing with https://www.x.com/docs/DOC-1160#id086970AK044

            using (var wc = new WebClient())
            {
                _payPalCredentials.SetOn(wc.QueryString);
                var openRead = wc.OpenRead("https://api-3t.sandbox.paypal.com/nvp");
            }
            return new PaymentResponse();
        }
    }
}