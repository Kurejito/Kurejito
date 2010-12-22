using Kurejito.Payments;
using Should;
using Xunit;

namespace Kurejito.Tests.Payments {
    public class AcceptAttributeTests {
        [Fact]
        public void DecoratedToAccept_Should_Return_True_For_Canadian_Dollar()
        {
            AcceptsAttribute.DecoratedToAccept<TestAcceptAttributeGateway>("GBR", "CAD", CardType.Visa).ShouldBeTrue();
        }

        [Fact]
        public void DecoratedToAccept_Should_Return_False_For_Costa_Rican_Colon()
        {
            AcceptsAttribute.DecoratedToAccept<TestAcceptAttributeGateway>("GBR", "CRC", CardType.Visa).ShouldBeFalse();
        }

        [Fact]
        public void DecoratedToAccept_With_Differently_Cased_Currency_Code_Should_Still_Return_True()
        {
            AcceptsAttribute.DecoratedToAccept<TestAcceptAttributeGateway>("GBR", "eUr", CardType.Visa).ShouldBeTrue();
        }

        [Fact]
        public void DecoratedToAccept_With_Differently_Cased_Country_Code_Should_Still_Return_True()
        {
            AcceptsAttribute.DecoratedToAccept<TestAcceptAttributeGateway>("GbR", "EUR", CardType.Visa).ShouldBeTrue();
        }

        [Fact]
        public void Constructor_Should_Trim_Spaces_From_Currency_Codes()
        {
            AcceptsAttribute.DecoratedToAccept<TestAcceptAttributeGateway>("GBR", "USD", CardType.Visa).ShouldBeTrue();
        }

        [AcceptsAttribute("GBR", "AUD,CAD,CZK,DKK,EUR, USD ", CardType.Visa, CardType.MasterCard)]
        class TestAcceptAttributeGateway {
            
        }
    }
}