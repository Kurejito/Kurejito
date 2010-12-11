using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Kurejito.Extensions;

namespace Kurejito.Validation {
    ///<summary>
    ///</summary>
    public interface IValidate<T> {
        /// <summary>
        ///   Validates the specified t.
        /// </summary>
        /// <param name = "t">The t.</param>
        /// <returns></returns>
        ValidationResult Validate(T t);//TODO this method name on this interface sucks.
    }

    /// <summary>
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    internal class RequiredValidator<T> : IValidate<T> {
        //TODO put these fiels and the construction in their own object so validators can share.
        private readonly Func<string> messageGetter;
        private readonly Func<T, object> propertyGetter;
        private readonly MemberInfo propertyMemberName;
        private readonly MemberInfo resourceMemberName;

        public RequiredValidator(Expression<Func<T, object>> valueFunc, Expression<Func<string>> failMessageFunc) {
            if (valueFunc == null) throw new ArgumentNullException("valueFunc");
            if (failMessageFunc == null) throw new ArgumentNullException("failMessageFunc");
            this.propertyGetter = valueFunc.Compile();
            this.propertyMemberName = valueFunc.GetMember();
            this.resourceMemberName = failMessageFunc.GetMember();
            this.messageGetter = failMessageFunc.Compile();
        }

        #region IValidate<T> Members

        public ValidationResult Validate(T t) {
            var value = this.propertyGetter(t);

            if (value == null || value.ToString().IsNullOrWhiteSpace()) {
                return new ValidationResult(new ValidationFailure(this.propertyMemberName.Name, this.resourceMemberName.Name, String.Format(this.messageGetter(), this.propertyMemberName.Name)));
            }
            return ValidationResult.Success;
        }

        #endregion
    }
}