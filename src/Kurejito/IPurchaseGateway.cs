using Kurejito.Payments;

namespace Kurejito {

	/// <summary>Defines behaviour of gateways that support purchase transactions, where the card is debited immediately.</summary>
	public interface IPurchaseGateway {
		/// <summary>Attempts to debit the specified amount from the supplied payment card.</summary>
		/// <param name="merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
		/// <param name="amount">The amount of money to be debited from the payment card</param>
		/// <param name="currency">The ISO4217 currency code of the currency to be used for this transaction.</param>
		/// <param name="card">An instance of <see cref="PaymentCard"/> containing the customer's payment card details.</param>
		/// <returns>A <see cref="PaymentResponse"/> indicating whether the transaction succeeded.</returns>
		PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card);
	}
}
