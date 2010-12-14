using System;
using Kurejito.Payments;

namespace Kurejito.Tests.Payments {
    public class TestPaymentCards {
        //MAYBE make this a card builder instead (e.g. PaymentCard.InDate().BadCv2() etc)
        public static PaymentCard VisaValid {
            get { return new PaymentCard("BEN TAYLOR", "4716034283508634", new CardDate(10, DateTime.Now.Year + 2), "123", CardType.Visa); }
        }
    }
}