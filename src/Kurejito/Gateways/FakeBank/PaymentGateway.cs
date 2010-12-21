using Kurejito.Payments;

namespace Kurejito.Gateways.FakeBank {
	/// <summary>
    /// Provides a fake, in-memory, stateless simulated payment provider for testing code that uses a <see cref="IPurchase" />.
	/// </summary>
	public class PaymentGateway : IPurchase {
        /// <summary>
        /// Attempts to debit the specified amount from the supplied payment card.
        /// </summary>
        /// <param name="merchantReference">An alphanumeric reference supplied by the merchant that uniquely identifies this transaction</param>
        /// <param name="amount">The amount of money to be debited from the payment card (includes the ISO4217 currency code).</param>
        /// <param name="card">An instance of <see cref="PaymentCard"/> containing the customer's payment card details.</param>
        /// <returns>
        /// A <see cref="PaymentResponse"/> indicating whether the transaction succeeded.
        /// </returns>
		public PaymentResponse Purchase(string merchantReference, Money amount, PaymentCard card) {
            if (amount > 10.0m)
            {
                return (new PaymentResponse()
                {
                    Status = PaymentStatus.Declined,
                    Reason = "Sorry - FakeBank only accepts transactions under a tenner!"
                });
            }
            return (new PaymentResponse()
            {
                Status = PaymentStatus.Ok,
                Reason = "Payment successful"
            });
        }
	}
}

