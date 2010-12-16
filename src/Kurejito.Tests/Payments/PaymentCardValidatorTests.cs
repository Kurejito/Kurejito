using Kurejito.Payments;
using Kurejito.Tests.Extensions;
using Kurejito.Validation;
using Should;
using Xunit;
using Xunit.Extensions;

namespace Kurejito.Tests.Payments {
    public class PaymentCardValidatorTests {
        private static ValidationResult Validate(PaymentCard paymentCard) {
            return new PaymentCardValidator()
                .Validate(paymentCard);
        }

        [Fact]
        public void With_No_Data_IsValid_Should_Be_False() {
            Validate(new PaymentCard()).IsValid.ShouldBeFalse();
        }

        [Fact]
        public void CardHolder_Cannot_Be_Blank() {
            Validate(new PaymentCard())
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardHolder, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }

        [Fact]
        public void CV2_Cannot_Be_Blank() {
            Validate(new PaymentCard())
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CV2, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }
        
        [Fact(Skip="TODO get validator going for enums.")]
        public void CardType_Cannot_Be_Blank()
        {
            Validate(new PaymentCard())
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardType, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }

        [Fact]
        public void ExpiryDate_Cannot_Be_Blank() {
            Validate(new PaymentCard())
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.ExpiryDate, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("456")]
        public void Invalid_Card_Numbers_Should_Fail_Luhn(string cardNumber) {
            Validate(new PaymentCard {CardNumber = cardNumber})
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.Payment_Card_Failed_Luhn_Check);
        }

        [Theory]
        [InlineData("378282246310005")]//Amex
        [InlineData("30569309025904")]//Diners
        [InlineData("5555555555554444")]//Mastercard
        [InlineData("4111111111111111")]//Visa
        public void Valid_Card_Numbers_Should_Not_Fail_Luhn(string cardNumber)
        {
            Validate(new PaymentCard { CardNumber = cardNumber })
                .Failures
                .ShouldNotContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.Payment_Card_Failed_Luhn_Check);
        }

    }
}