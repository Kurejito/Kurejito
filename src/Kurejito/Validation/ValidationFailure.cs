using System;

namespace Kurejito.Validation {
    /// <summary>
    /// Describes a specific validation failure that forms part of a <see cref="ValidationResult"/>.
    /// </summary>
    public class ValidationFailure {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationFailure"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="resourceMemberName">Name of the resource member.</param>
        /// <param name="message">The message.</param>
        public ValidationFailure(string propertyName, string resourceMemberName, string message) {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            if (resourceMemberName == null) throw new ArgumentNullException("resourceMemberName");
            if (message == null) throw new ArgumentNullException("message");
            this.PropertyName = propertyName;
            this.MessageResourceName = resourceMemberName;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets or sets the name of the message resource.
        /// </summary>
        /// <value>The name of the message resource.</value>
        public string MessageResourceName { get; private set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; private set; }
    }
}