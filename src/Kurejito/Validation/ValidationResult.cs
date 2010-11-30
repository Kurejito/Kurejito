using System.Collections.Generic;
using System.Linq;

namespace Kurejito.Validation {
    /// <summary>
    ///   Describes whether a validation operation was successful or not.  Gives additional details where relevant.
    /// </summary>
    public class ValidationResult {
        private readonly List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ValidationResult" /> class.
        /// </summary>
        /// <param name = "validationFailures">The validation failures.</param>
        public ValidationResult(IEnumerable<ValidationFailure> validationFailures) {
            if (validationFailures == null)
                return;
            this.validationFailures.AddRange(validationFailures.Where(f => f != null));
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid {
            get { return this.validationFailures.Count == 0; }
        }

        ///<summary>
        ///</summary>
        public IList<ValidationFailure> Failures {
            get { return this.validationFailures; }
        }

        /// <summary>
        ///   Froms the running.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "paymentCard">The payment card.</param>
        /// <param name = "validators">The validators.</param>
        /// <returns></returns>
        public static ValidationResult FromRunning<T>(T paymentCard, IList<IValidate<T>> validators) {
            return new ValidationResult(validators.Select(v => v.TryFail(paymentCard)).Where(f => f != null));
        }
    }
}