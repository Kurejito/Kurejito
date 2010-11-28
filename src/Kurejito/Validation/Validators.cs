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
    internal class RequiredValidator<T> : IValidate<T>
    {
        //TODO put these fiels and the construction in their own object so validators can share.
        private readonly Func<T, object> propertyGetter;
        private readonly MemberInfo propertyMemberName;
        private readonly MemberInfo resourceMemberName;
        private readonly Func<string> messageGetter;

        public RequiredValidator(Expression<Func<T, object>> expression, Expression<Func<string>> msgGetter)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            if (msgGetter == null) throw new ArgumentNullException("msgGetter");
            this.propertyGetter = expression.Compile();
            this.propertyMemberName = expression.GetMember();
            this.resourceMemberName = msgGetter.GetMember();
            this.messageGetter = msgGetter.Compile();
        }
   
        public ValidationFailure TryFail(T t) {
            return this.propertyGetter(t).ToString().IsNullOrWhiteSpace() ? new ValidationFailure(propertyMemberName.Name, resourceMemberName.Name, String.Format(messageGetter(), propertyMemberName.Name)) : null;
        }
    }
}