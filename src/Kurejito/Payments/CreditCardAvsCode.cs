namespace Kurejito.Payments {
    /// <summary>
    ///   Address Verification System Codes for credit cards (Visa, Mastercard, Discover and Amex).
    ///   Source: http://en.wikipedia.org/wiki/Address_Verification_System
    /// </summary>
    public enum CreditCardAvsCode {
        /// <summary>
        ///   Street address matches, but 5-digit and 9-digit postal code do not match.  Standard domestic.
        /// </summary>
        A,
        /// <summary>
        ///   Street address matches, but postal code not verified.	Standard international
        /// </summary>
        B,
        /// <summary>
        ///   Street address and postal code do not match.	Standard international
        /// </summary>
        C,
        /// <summary>
        ///   Street address and postal code match. Code "M" is equivalent.	Standard international
        /// </summary>
        D,
        /// <summary>
        ///   AVS data is invalid or AVS is not allowed for this card type.	Standard domestic
        /// </summary>
        E,
        /// <summary>
        ///   Card member's name does not match, but billing postal code matches.	American Express only
        /// </summary>
        F,
        /// <summary>
        ///   Non-U.S. issuing bank does not support AVS.	Standard international
        /// </summary>
        G,
        /// <summary>
        ///   Card member's name does not match. Street address and postal code match.	American Express only
        /// </summary>
        H,
        /// <summary>
        ///   Address not verified.	Standard international
        /// </summary>
        I,
        /// <summary>
        ///   Card member's name, billing address, and postal code match.	American Express only
        /// </summary>
        J,
        /// <summary>
        ///   Card member's name matches but billing address and billing postal code do not match.	American Express only
        /// </summary>
        K,
        /// <summary>
        ///   Card member's name and billing postal code match, but billing address does not match.	American Express only
        /// </summary>
        L,
        /// <summary>
        ///   Street address and postal code match. Code "D" is equivalent.	Standard international
        /// </summary>
        M,
        /// <summary>
        ///   Street address and postal code do not match.	Standard domestic
        /// </summary>
        N,
        /// <summary>
        ///   Card member's name and billing address match, but billing postal code does not match.	American Express only
        /// </summary>
        O,
        /// <summary>
        ///   Postal code matches, but street address not verified.	Standard international
        /// </summary>
        P,
        /// <summary>
        ///   Card member's name, billing address, and postal code match.	American Express only
        /// </summary>
        Q,
        /// <summary>
        ///   System unavailable.	Standard domestic
        /// </summary>
        R,
        /// <summary>
        ///   Bank does not support AVS.	Standard domestic
        /// </summary>
        S,
        /// <summary>
        ///   Card member's name does not match, but street address matches.	American Express only
        /// </summary>
        T,
        /// <summary>
        ///   Address information unavailable. Returned if the U.S. bank does not support non-U.S. AVS or if the AVS in a U.S. bank is not functioning properly.	Standard domestic
        /// </summary>
        U,
        /// <summary>
        ///   Card member's name, billing address, and billing postal code match.	American Express only
        /// </summary>
        V,
        /// <summary>
        ///   Street address does not match, but 9-digit postal code matches.	Standard domestic
        /// </summary>
        W,
        /// <summary>
        ///   Street address and 9-digit postal code match.	Standard domestic
        /// </summary>
        X,
        /// <summary>
        ///   Street address and 5-digit postal code match.	Standard domestic
        /// </summary>
        Y,
        /// <summary>
        ///   Street address does not match, but 5-digit postal code matches.	Standard domestic
        /// </summary>
        Z
    }
}