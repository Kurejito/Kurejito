using System;
using System.Linq;
using Kurejito.Validation;

namespace Kurejito.Payments {
    /// <summary>
    /// Responsible for validation <see cref = "PaymentCard" /> instances.
    /// Reasonable support for validating Visa, Mastercard and American Express.
    /// Based on information from the following sources:
    /// * http://stackoverflow.com/questions/1463252/creditcard-verification-with-regex
    /// * http://www.beachnet.com/~hstiles/cardtype.html
    /// </summary>
    public class PaymentCardValidator : ValidatorBase<PaymentCard> {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PaymentCardValidator" /> class.
        /// </summary>
        public PaymentCardValidator() {
            this.AddRule(pc => pc.CardNumber, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddRule(pc => pc.CardNumber, IsLuhnValid, () => Payments.PaymentCard_CardNumber_Failed_Luhn_Check);
            this.AddRule(pc => pc.CardHolder, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddRule(pc => pc.CV2, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddRule(pc => pc.ExpiryDate, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            //TODO//this.AddRule(pc => pc.CardType, IsPopulated, () => Payments.PaymentCard_BlankProperty);

            //Mastercard 
            this.AddConditionalRule(
                pc => pc.CardType == CardType.Mastercard,
                pc => pc.CardNumber,
                cn => MatchesRegex(cn, "^5[1-5][0-9]{14}$"),
                () => Payments.PaymentCard_CardNumber_Fails_CardType_Rules);

            //Visa
            this.AddConditionalRule(
                pc => pc.CardType == CardType.Visa,
                pc => pc.CardNumber,
                cn => MatchesRegex(cn, "^4[0-9]{12}(?:[0-9]{3})?$"),
                () => Payments.PaymentCard_CardNumber_Fails_CardType_Rules);

            //Amex
            this.AddConditionalRule(
                pc => pc.CardType == CardType.AmericanExpress,
                pc => pc.CardNumber,
                cn => MatchesRegex(cn, "^3[47][0-9]{13}$"),
                () => Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }

        /// <summary>
        ///   Standard Luhn check.
        ///   See http://en.wikipedia.org/wiki/Luhn_algorithm for details and source.
        /// </summary>
        private static bool IsLuhnValid(object value) {
            if (value == null)
                return false;

            return value.ToString().Where(Char.IsDigit).Reverse()
                       .SelectMany((c, i) => ((c - '0') << (i & 1)).ToString())
                       .Sum(c => c - '0')%10 == 0;
        }
    }
}