using System;
using System.Collections.Generic;

namespace Kurejito.Payments
{
    /// <summary>
    /// Describes whether a validation operation was successful or not.  Gives additional details where relevant.
    /// </summary>
    public class ValidationResult
    {
        readonly IList<ValidationFailure> validationFailures = new List<ValidationFailure>();

        /// <summary>
        /// Adds the specified validation failure.
        /// </summary>
        /// <param name="validationFailure">The validation failure.</param>
        internal void AddFailure(string validationFailure)
        {
            if (validationFailure == null) throw new ArgumentNullException("validationFailure");
            validationFailures.Add(validationFailure);
        }
    }

    /// <summary>
    /// Describes a specific validation failure that forms part of a <see cref="ValidationResult"/>.
    /// </summary>
    public class ValidationFailure
    {
        public string Message { get; set; }

        public ValidationFailure(string message)
        {
            Message = message;
            throw new NotImplementedException();
        }
    }
}