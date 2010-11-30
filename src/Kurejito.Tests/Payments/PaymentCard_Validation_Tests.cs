using Kurejito.Payments;
using Kurejito.Tests.Extensions;
using Should;
using Xunit;

namespace Kurejito.Tests.Payments {
    public class PaymentCard_Validation_Tests {
        [Fact]
        public void With_No_Data_IsValid_Should_Be_False() {
            new PaymentCard().Validate().IsValid.ShouldBeFalse();
        }

        [Fact]
        public void CardHolder_Cannot_Be_Blank() {
            new PaymentCard().Validate().Failures.ShouldContainFailure<PaymentCard>(pc => pc.CardHolder, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }

        [Fact]
        public void CV2_Cannot_Be_Blank()
        {
            new PaymentCard().Validate().Failures.ShouldContainFailure<PaymentCard>(pc => pc.CV2, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }

        [Fact]
        public void ExpiryDate_Cannot_Be_Blank()
        {
            new PaymentCard().Validate().Failures.ShouldContainFailure<PaymentCard>(pc => pc.ExpiryDate, () => Kurejito.Payments.Payments.PaymentCard_BlankProperty);
        }

    }
}