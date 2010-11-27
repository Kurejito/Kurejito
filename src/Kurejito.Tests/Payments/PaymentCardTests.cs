using Kurejito.Payments;
using Xunit;

namespace Kurejito.Tests.Payments {
    public class PaymentCardTests {
        [Fact]
        public void TestName() {
            new PaymentCard().Validate().IsValid == false;
                
        }

        [Fact]
        public void CardHolder_Cannot_Be_Blank()
        {
            new PaymentCard().Validate().Failures.Contains

        }
    }
}