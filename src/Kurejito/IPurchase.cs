using Kurejito.Payments;
///<summary>The root-level namespace for the Kurejito payment gateway API and for provider-specific modules
/// that implement this API against various commercial internet payment providers.</summary>
namespace Kurejito {

	/// <summary>Providers that support purchases - straightforward payments where the money is debited immediately - should implement this interface.</summary>
	public interface IPurchase : IAccept {
		/// <summary>Attempts to debit the specified amount from the supplied payment card.</summary>
		/// <param name="merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
        /// <param name="amount">The amount of money to be debited from the payment card (includes the ISO4217 currency code).</param>
		/// <param name="card">An instance of <see cref="PaymentCard"/> containing the customer's payment card details.</param>
		/// <returns>A <see cref="PaymentResponse"/> indicating whether the transaction succeeded.</returns>
		PaymentResponse Purchase(string merchantReference, Money amount, PaymentCard card);
	}
}
