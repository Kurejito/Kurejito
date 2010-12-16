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
            var failureCount = FindMatchingFailures(propName, resourceName, failures);
            Assert.Equal(1, failureCount, String.Format("No failure found or property {0} with resource named {1}.", propName.GetMember().Name, resourceName.GetMember().Name));
        }

        public static void ShouldNotContainFailure<T>(this IEnumerable<ValidationFailure> failures, Expression<Func<T, object>> propName, Expression<Func<string>> resourceName)
        {
            var failureCount = FindMatchingFailures(propName, resourceName, failures);
            Assert.Equal(0, failureCount, String.Format("Failure found for property {0} with resource named {1}.", propName.GetMember().Name, resourceName.GetMember().Name));
        }

        private static int FindMatchingFailures<T>(Expression<Func<T, object>> propName, Expression<Func<string>> resourceName, IEnumerable<ValidationFailure> failures) {
            if (propName == null) throw new ArgumentNullException("propName");
            if (resourceName == null) throw new ArgumentNullException("resourceName");
            return failures.Where(f => f.PropertyName.Equals(propName.GetMember().Name) && f.MessagePropertyName.Equals(resourceName.GetMember().Name)).Count();
        }
    }
}
