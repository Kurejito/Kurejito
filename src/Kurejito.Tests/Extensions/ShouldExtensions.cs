using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Kurejito.Validation;
using Kurejito.Extensions;
using Should.Core.Assertions;

namespace Kurejito.Tests.Extensions
{
    internal static class ShouldExtensions
    {
        public static void ShouldContainFailure<T>(this IEnumerable<ValidationFailure> failures, Expression<Func<T, object>> propName, Expression<Func<string>> resourceName) {
            if (propName == null) throw new ArgumentNullException("propName");
            if (resourceName == null) throw new ArgumentNullException("resourceName");
            var match = failures.Where(f => f.PropertyName.Equals(propName.GetMember().Name) && f.MessagePropertyName.Equals(resourceName.GetMember().Name));
            Assert.NotNull(match, String.Format("No failure found or property {0} with resource named {1}.", propName.GetMember().Name, resourceName.GetMember().Name));
        }
    }
}
