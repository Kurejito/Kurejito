using Kurejito.Validation;
using Should;
using Xunit;

namespace Kurejito.Tests.Validation {
    public class RequiredValidatorTests {
        [Fact]
        public void Failure_PropertyName_Should_Match_Object_Property_Name() {
            var requiredValidator = new RequiredValidator<ThingToValidate>(t => t.ThingNumber, () => TestValidation.ThingNumberRequired);
            requiredValidator.Validate(new ThingToValidate()).Failures[0].PropertyName.ShouldEqual("ThingNumber");
        }
    }

    internal class ThingToValidate {
        public int? ThingNumber { get; set; }
    }
}