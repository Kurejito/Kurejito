using System;
using System.Linq.Expressions;
using Kurejito.Extensions;

namespace Kurejito.Validation {
    internal class ValidationRule<T> : IValidate<T> {
        private Func<T, bool> conditional;
        private Func<object, bool> validator;
        private Func<string> messageGetter;
        private Func<T, object> valueGetter;
        private string propertyName;
        private string resourceName;

        public static ValidationRule<T> Create(Func<T, bool> ifCondition, Expression<Func<T, object>> valueGetter, Func<object, bool> validator, Expression<Func<string>> messageGetter) {
            var propertyMemberInfo = valueGetter.GetMember();
            if (propertyMemberInfo == null)
                throw new ArgumentException("");

            var resourceMemberInfo = messageGetter.GetMember();
            if (resourceMemberInfo == null)
                throw new ArgumentException("");

            return new ValidationRule<T> {
                                             conditional = ifCondition,
                                             valueGetter = valueGetter.Compile(),
                                             propertyName = propertyMemberInfo.Name,
                                             resourceName = resourceMemberInfo.Name,
                                             messageGetter = messageGetter.Compile(),
                                             validator = validator
                                         };
        }

        public ValidationResult Validate(T t) {

            if (!conditional(t) || validator(this.valueGetter(t)))
                return ValidationResult.Success;
            return new ValidationResult(new ValidationFailure(propertyName, resourceName, String.Format(messageGetter(), propertyName)));
        }
    }
}