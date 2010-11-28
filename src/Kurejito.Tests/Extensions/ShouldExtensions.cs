using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Kurejito.Validation;

namespace Kurejito.Tests.Extensions
{
    internal static class ShouldExtensions
    {
        public static void ShouldContainFailure<T>(this IEnumerable<ValidationFailure> failures, Expression<Func<T, object>> propName, Expression<Func<string>> resourceName) {
            
        }
    }
}
