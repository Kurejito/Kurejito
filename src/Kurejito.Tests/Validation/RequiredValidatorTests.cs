using Kurejito.Validation;
using Should;
using Xunit;

namespace Kurejito.Tests.Validation {
    public class RequiredValidatorTests {
        private readonly ValidationResult validationResult;

        public RequiredValidatorTests() {
            var requiredValidator = new RequiredValidator<ThingToValidate>(t => t.ThingNumber, () => TestValidation.ThingNumberRequired);
            validationResult = requiredValidator.Validate(new ThingToValidate());
        }
      
        [Fact]
        public void Failure_PropertyName_Should_Match_Object_Property_Name() {
            validationResult.Failures[0].PropertyName.ShouldEqual("ThingNumber");
        }

        [Fact]
        public void Failure_MessagePropertyName_Should_Match_Message_Object_Property_Name()
        {
            validationResult.Failures[0].MessagePropertyName.ShouldEqual("ThingNumberRequired");
        }
    }

    internal class ThingToValidate {
        public int? ThingNumber { get; set; }
    }
}