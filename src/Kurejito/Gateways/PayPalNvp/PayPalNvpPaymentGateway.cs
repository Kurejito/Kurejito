using System;
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
            throw new NotImplementedException();
        }
    }

    public class PayPalCredentials
    {
        public PayPalCredentials(string username, string password, string signature)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");
            if (signature == null) throw new ArgumentNullException("signature");
            Username = username;
            Password = password;
            Signature = signature;
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Signature { get; private set; }
    }
}