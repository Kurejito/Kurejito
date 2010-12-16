/*using Kurejito.Validation;
using Should;
using Xunit;

namespace Kurejito.Tests.Validation {
    public class RequiredValidatorTests_WhenPropertyNotSet {
        private readonly ValidationResult validationResult;

        public RequiredValidatorTests_WhenPropertyNotSet() {
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

        [Fact]
        public void ValidationResult_IsValid_Should_Be_False()
        {
            validationResult.IsValid.ShouldEqual(false);
        }

        [Fact]
        public void ValidationResult_Failures_Count_Should_Be_One()
        {
            validationResult.Failures.Count.ShouldEqual(1);
        }
    }

    internal class ThingToValidate {
        public int? ThingNumber { get; set; }
    }
}*/