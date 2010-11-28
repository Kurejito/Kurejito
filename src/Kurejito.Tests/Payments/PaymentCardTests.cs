using Kurejito.Payments;
using Should;
using Xunit;

namespace Kurejito.Tests.Payments {
    public class PaymentCardTests {
        [Fact]
        public void TestName() {
            new PaymentCard().Validate().IsValid.ShouldBeFalse();
        }

        [Fact]
        public void CardHolder_Cannot_Be_Blank() {
            new PaymentCard().Validate().Failures.Count.ShouldEqual(3);
        }
    }
}