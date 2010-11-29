
namespace Kurejito {
	///<summary>Defines the various payment status values that can be returned by a payment gateway.</summary>
	///<remarks><para>Because different gateways use different response codes, Kurejito will translate these
	/// into a standard set of responses.</para>
	/// <para>As a very general rule, Kurejito tries to translate the underlying response codes so that
	/// an <see cref="PaymentStatus.Error"/> status might succeed if you try it again, but an <see cref="PaymentStatus.Invalid" />
	/// status means the transaction is not valid and won't succeed no matter how many times you try it.</para>
	/// </remarks>
	public enum PaymentStatus {
		///<summary>The status of a payment that hasn't been sent to the gateway yet.</summary>
		Undefined,
		///<summary>The payment was taken successfully.</summary>
		Ok,
		///<summary>The payment was declined by the card's issuing bank</summary>
		Declined,
		///<summary>The payment request was not valid - e.g. incorrect card number or card already expired.</summary>
		Invalid,
		///<summary>The payment has been referred for authorization - call the authorization centre.</summary>
		Referred,
		/// <summary>The payment failed because of a system error, communications error or other technical fault.</summary>
		Error
	}
}
