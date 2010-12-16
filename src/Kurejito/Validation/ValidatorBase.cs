using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Kurejito.Extensions;

namespace Kurejito.Validation {
    /// <summary>
    ///   Base class for types that wish to specify <see cref = "ValidationRule{T}" /> instances for a {T}.
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    public abstract class ValidatorBase<T> : IValidate<T> {
        private readonly IList<ValidationRule<T>> validationRules = new List<ValidationRule<T>>();

        #region IValidate<T> Members

        public ValidationResult Validate(T t) {
            return new ValidationResult(this.validationRules.Select(v => v.Validate(t)).SelectMany(vr => vr.Failures));
        }

        #endregion

        protected void AddValidation(Expression<Func<T, object>> propertyGetter, Func<object, bool> validator, Expression<Func<string>> messageGetter) {
            this.AddConditionalValidation(t => true, propertyGetter, validator, messageGetter);
        }

        protected void AddConditionalValidation(Func<T, bool> ifTrue, Expression<Func<T, object>> propertyGetter, Func<object, bool> validator, Expression<Func<string>> messageGetter) {
            var validationSite = ValidationRule<T>.Create(ifTrue, propertyGetter, validator, messageGetter);
            this.validationRules.Add(validationSite);
        }

        protected static bool IsPopulated(object value) {
            if (value == null)
                return false;

            return !value.ToString().IsNullOrWhiteSpace();
        }

        protected static bool MatchesRegex(object value, string regex) {
            return value != null && new Regex(regex).IsMatch(value.ToString()); //REVIEW newing up regex every time.
        }
    }
}