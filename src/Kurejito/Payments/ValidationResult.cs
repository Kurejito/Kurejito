using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kurejito.Extensions;

namespace Kurejito.Payments {
    /// <summary>
    /// Describes whether a validation operation was successful or not.  Gives additional details where relevant.
    /// </summary>
    public class ValidationResult {
        private readonly List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="validationFailures">The validation failures.</param>
        public ValidationResult(params ValidationFailure[] validationFailures) {
            if (validationFailures == null)
                return;
            this.validationFailures.AddRange(validationFailures.Where(f => f != null));
        }
    }

    /// <summary>
    /// Describes a specific validation failure that forms part of a <see cref="ValidationResult"/>.
    /// </summary>
    public abstract class ValidationFailure {
   
    }

    /// <summary>
    /// 
    /// </summary>
    public class BlankPropertyValidationFailure : ValidationFailure {
        /// <summary>
        /// Tries the fail.
        /// </summary>
        /// <param name="propertyAccessor">The property accessor.</param>
        /// <returns></returns>
        public static ValidationFailure TryFail(Func<string> propertyAccessor) {
            if (propertyAccessor().IsNullOrWhiteSpace())
                return new BlankPropertyValidationFailure(propertyAccessor.GetMember())
        }
    }
}