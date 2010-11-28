using System;
using System.Linq.Expressions;
using System.Reflection;
using Kurejito.Extensions;

namespace Kurejito.Validation {
    ///<summary>
    ///</summary>
    public interface IValidate<T> {
        /// <summary>
        /// Validates the specified t.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        ValidationFailure TryFail(T t);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class RequiredValidator<T> : IValidate<T> {
        //TODO put these fiels and the construction in their own object so validators can share.
        private readonly Func<string> messageGetter;
        private readonly Func<T, object> propertyGetter;
        private readonly MemberInfo propertyMemberName;
        private readonly MemberInfo resourceMemberName;

        public RequiredValidator(Expression<Func<T, object>> expression, Expression<Func<string>> msgGetter) {
            if (expression == null) throw new ArgumentNullException("expression");
            if (msgGetter == null) throw new ArgumentNullException("msgGetter");
            this.propertyGetter = expression.Compile();
            this.propertyMemberName = expression.GetMember();
            this.resourceMemberName = msgGetter.GetMember();
            this.messageGetter = msgGetter.Compile();
        }

        #region IValidate<T> Members

        public ValidationFailure TryFail(T t) {
            object value = this.propertyGetter(t);

            if (value == null || value.ToString().IsNullOrWhiteSpace())
                return new ValidationFailure(this.propertyMemberName.Name, this.resourceMemberName.Name, String.Format(this.messageGetter(), this.propertyMemberName.Name));

            return null;
        }

        #endregion
    }
}