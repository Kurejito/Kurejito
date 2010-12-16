using System;
using System.Linq;
using Kurejito.Validation;

namespace Kurejito.Payments {
    /// <summary>
    ///   Responsible for validation <see cref = "PaymentCard" /> instances.
    ///   Currently supports Visa and Mastercard.
    /// </summary>
    public class PaymentCardValidator : ValidatorBase<PaymentCard> {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PaymentCardValidator" /> class.
        /// </summary>
        public PaymentCardValidator() {
            this.AddValidation(pc => pc.CardNumber, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddValidation(pc => pc.CardNumber, IsLuhnValid, () => Payments.Payment_Card_Failed_Luhn_Check);
            this.AddValidation(pc => pc.CardHolder, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddValidation(pc => pc.CV2, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddValidation(pc => pc.ExpiryDate, IsPopulated, () => Payments.PaymentCard_BlankProperty);
            this.AddValidation(pc => pc.CardType, IsPopulated, () => Payments.PaymentCard_BlankProperty);

            //Mastercard specific
            this.AddConditionalValidation(pc => pc.CardType == CardType.Mastercard, pc => pc.CardNumber, o => MatchesRegex(o, ""), () => Payments.PaymentCard_Bad_Mastercard_Number);

            //TODO Visa specific
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