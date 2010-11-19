using Kurejito.Payments;

namespace Kurejito.Gateways.FakeBank {
	/// <summary>
	/// Provides a fake, in-memory, stateless simulated payment provider for testing code that uses a <see cref="IPaymentGateway" />.
	/// </summary>
	public class PaymentGateway : IPurchaseGateway {
		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
			if (amount > 10.0m) {
				return (new PaymentResponse() {
					Status = PaymentStatus.Declined,
					Reason = "Sorry - FakeBank only accepts transactions under a tenner!"
				});
			}
			return (new PaymentResponse() {
				Status = PaymentStatus.OK,
				Reason = "Payment successful"
			});
		}

	}
}

