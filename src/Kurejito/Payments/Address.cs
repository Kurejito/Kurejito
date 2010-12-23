using System;
using Kurejito.Validation;

namespace Kurejito.Payments {
    /// <summary>
    /// International representation of a customers address.  Billing or shipping.
    /// </summary>
    public class Address : IValidate<Address> {
        //NOTE: This initial set of properties are based on the PayPal NVP API fields.
        //TODO: Enforce field lengths on Address to lowest common denominator in order to enable switching?

        /// <summary>
        /// Gets or sets the first name of the person at this address.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }//NOTE: PayPal 25 single byte chars

        /// <summary>
        /// Gets or sets the last name of the person at this address.
        /// </summary>
        /// <value>The last name.</value>
        
        public string LastName { get; set; }//NOTE: PayPal 25 single byte chars
        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        
        public string Street { get; set; }//NOTE: PayPal 100 single byte chars
        /// <summary>
        /// Gets or sets the street2.
        /// </summary>
        /// <value>The street2.</value>
        public string Street2 { get; set; }//NOTE: PayPal 100 single byte chars
        
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City { get; set; }//NOTE: PayPal 40 single byte chars
        
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }//NOTE: PayPal 40 single byte chars
        
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>The country code.</value>
        public string CountryCode { get; set; }//NOTE: PayPal 2 single byte chars
        
        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>The postal code.</value>
        public string PostalCode { get; set; }//NOTE: PayPal 20 single byte chars//No Zip here!
        
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNumber { get; set; }//NOTE: PayPal 20 single byte chars
        
        /// <summary>
        /// Determines whether this <see cref="Address"/> instance is considered valid.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public ValidationResult Validate(Address t) {
            throw new NotImplementedException("Need to add validation for addresses based on CountryCode etc.");
        }
    }
}