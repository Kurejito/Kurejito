using System;
using Kurejito.Payments;

namespace Kurejito {
    internal interface IAuthoriseAndCapture {
        PaymentResponse Authorise(string merchantReference, Money amount, PaymentCard card);
        PaymentResponse Capture();
    }
}