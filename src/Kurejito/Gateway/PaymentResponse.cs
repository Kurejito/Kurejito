using System;
using System.Collections.Generic;
using System.Text;

namespace Kurejito.Gateway {
	/// <summary>
	/// The result returned by the <see cref="IPaymentGateway" /> in response to a payment request.
	/// </summary>
	public class PaymentResponse {
		/// <summary>An <see cref="PaymentStatus" /> indicating whether the payment request succeeded.</summary>
		public PaymentStatus Status { get; set; }
		/// <summary>A human-readable description of the payment status, including the reason for failure if applicable.</summary>
		/// <remarks>This should be suitable for passing to the customer explaining why their payment succeeded or failed.</remarks>
		public string StatusDetail { get; set; }
	}
}
