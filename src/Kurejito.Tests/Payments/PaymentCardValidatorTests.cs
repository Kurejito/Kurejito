using Kurejito.Payments;
using Kurejito.Tests.Extensions;
using Kurejito.Validation;
using Should;
using Xunit;
using Xunit.Extensions;

namespace Kurejito.Tests.Payments {
    public class PaymentCardValidatorTests {
        private static ValidationResult Validate(PaymentCard paymentCard) {
            var validationResult = new PaymentCardValidator().Validate(paymentCard);
            return validationResult;
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
        [InlineData("1111111111111111")]
        [InlineData("2223334445556667")]
        public void Invalid_Card_Numbers_Should_Fail_Luhn(string cardNumber) {
            Validate(new PaymentCard {CardNumber = cardNumber})
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Failed_Luhn_Check);
        }

        [Theory]
        [InlineData("378282246310005")]//Amex
        [InlineData("371449635398431")]//Amex
        [InlineData("30569309025904")]//Diners
        [InlineData("5555555555554444")]//Mastercard
        [InlineData("5105105105105100")]//Mastercard
        [InlineData("4111111111111111")]//Visa
        public void Valid_Card_Numbers_Should_Not_Fail_Luhn(string cardNumber)
        {
            Validate(new PaymentCard { CardNumber = cardNumber })
                .Failures
                .ShouldNotContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Failed_Luhn_Check);
        }

        [Theory]
        [InlineData("4111111111111111")]//Visa
        [InlineData("378282246310005")]//Amex
        [InlineData("371449635398431")]//Amex
        [InlineData("30569309025904")]//Diners
        public void Validate_When_CardType_Mastercard_Should_Fail_When_CardNumber_Is_Not_MC_Format(string cardNumber)
        {
	        Validate(new PaymentCard(){CardType = CardType.Mastercard, CardNumber = cardNumber})
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }

        [Theory]
        [InlineData("5555555555554444")]//Mastercard
        [InlineData("5105105105105100")]//Mastercard
        public void Validate_When_CardType_Mastercard_Should_Not_Fail_When_CardNumber_Is_MC_Format(string cardNumber)
        {
            Validate(new PaymentCard() { CardType = CardType.Mastercard, CardNumber = cardNumber })
                .Failures
                .ShouldNotContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }

        [Theory]
        [InlineData("378282246310005")]//Amex
        [InlineData("371449635398431")]//Amex
        [InlineData("30569309025904")]//Diners
        [InlineData("5555555555554444")]//Mastercard
        [InlineData("5105105105105100")]//Mastercard
        public void Validate_When_CardType_Visa_Should_Fail_When_CardNumber_Is_Not_Visa_Format(string cardNumber)
        {
            //TODO unable to tell if this is the Visa fail or MC fail.  Need to format and check full string, or have diff message per card?
            Validate(new PaymentCard() { CardType = CardType.Visa, CardNumber = cardNumber })
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }

        [Theory]
        [InlineData("4111111111111111")]//Visa
        [InlineData("4012888888881881")]//Visa
        [InlineData("4222222222222")]//Visa (13 digits)
        public void Validate_When_CardType_Visa_Should_Not_Fail_When_CardNumber_Is_Visa_Format(string cardNumber)
        {
            //TODO unable to tell if this is the Visa fail or MC fail.  Need to format and check full string, or have diff message per card?
            Validate(new PaymentCard() { CardType = CardType.Visa, CardNumber = cardNumber })
                .Failures
                .ShouldNotContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }

        [Theory]
        [InlineData("30569309025904")]//Diners
        [InlineData("5555555555554444")]//Mastercard
        [InlineData("5105105105105100")]//Mastercard
        [InlineData("4111111111111111")]//Visa
        public void Validate_When_CardType_AmericanExpress_Should_Fail_When_CardNumber_Is_Not_AmericanExpress_Format(string cardNumber)
        {
            //TODO unable to tell if this is the Visa fail or MC fail.  Need to format and check full string, or have diff message per card?
            Validate(new PaymentCard() { CardType = CardType.AmericanExpress, CardNumber = cardNumber })
                .Failures
                .ShouldContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }

        [Theory]
        [InlineData("378282246310005")]//Amex
        [InlineData("371449635398431")]//Amex
        public void Validate_When_CardType_AmericanExpress_Should_Not_Fail_When_CardNumber_Is_AmericanExpress_Format(string cardNumber)
        {
            //TODO unable to tell if this is the Visa fail or MC fail.  Need to format and check full string, or have diff message per card?
            Validate(new PaymentCard() { CardType = CardType.AmericanExpress, CardNumber = cardNumber })
                .Failures
                .ShouldNotContainFailure<PaymentCard>(pc => pc.CardNumber, () => Kurejito.Payments.Payments.PaymentCard_CardNumber_Fails_CardType_Rules);
        }
    }
}