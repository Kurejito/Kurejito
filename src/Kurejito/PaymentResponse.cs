namespace Kurejito {
    /// <summary>When you submit a payment to a payment gateway, you'll get back a 
    /// <see cref="PaymentResponse"/>. This will tell you whether or not the payment 
    /// succeeded, and provide human- and machine-readable information about the status
    /// of the payment.</summary>
	/// <remarks>The <see cref="PaymentStatus" /> field should be used by your code. The <see cref="Reason" /> field should be a human-readable string that you can display directly to your end users to tell them what happened if something goes wrong.</remarks>
	public class PaymentResponse {
        /// <summary>An <see cref="PaymentStatus" /> that indicates whether the payment was successful, declined, rejected, or an error occurred.</summary>
        public PaymentStatus Status { get; set; }
        /// <summary>A human-readable description of the payment status, including the reason for failure if applicable.</summary>
        public string Reason { get; set; }
    }
}