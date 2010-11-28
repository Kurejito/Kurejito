
namespace Kurejito {
	///<summary>Defines the various payment status values that can be returned by a payment gateway.</summary>
	///<remarks>Because different gateways use different response codes, Kurejito will translate these
	/// into a standard set of responses.</remarks>
	public enum PaymentStatus {
		///<summary>The status of a payment that hasn't been sent to the gateway yet.</summary>
		/// <remarks>This value should never be returned by a live payment gateway.</remarks>
		Undefined,
		/// <summary>The payment was taken successfully.</summary>
		/// <remarks>This means that the payment, authentication, refund, etc. has worked as expected.</remarks>
		Ok,
		/// <summary>The payment was declined by the card's issuing bank</summary>
		Declined,
		/// <summary>The payment request was not valid - e.g. incorrect card number or card already expired.</summary>
		Invalid,
		/// <summary>The payment has been referred for authorization - call the authorization centre.</summary>
		/// <remarks>In automated systems, you probably want to treat this as a DECLINED response</remarks>
		Referred,
		///<summary>Something went wrong trying to process this payment</summary>
		///<remarks>This status should only happen if there's a problem with the system or a bug in your code. You should never get an Error response from valid code against a properly-functioning gateway.</remarks>
		Error
	}
}
