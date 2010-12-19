namespace Kurejito.Payments {
    /// <summary>
    ///   TODO Solo and Maestro have the Avs codes commented out below (from PayPal docs). Figure out how different schemes got together in the model, or just leave as string?
    /// </summary>
    public enum DebitCardAvsCode {
/*
        0 All the address information matched. All information matched
        1 None of the address information
        matched.
        None
        NOTE: The transaction is declined.
        2 Part of the address information
        matched.
        Partial
        3 The merchant did not provide AVS
        information. Not processed.
        Not applicable
        4 Address not checked, or acquirer had
        no response. Service not available.
        Not applicable
        Null No AVS response was obtained.
        Default value of field.
        Not applicable
*/
    }
}