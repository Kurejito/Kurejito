using Kurejito.Payments;
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
            new PaymentCard().Validate().Failures.Count.ShouldEqual(3);
        }
    }
}