using Kurejito.Payments;

namespace Kurejito {
    /// <summary>
    /// Interface implemented by providers that support an authorise and capture payment model.
    /// </summary>
    public interface IAuthoriseAndCapture : IAccept {
        /// <summary>
        /// Authorise <paramref name="amount"/> for later Capture.
        /// </summary>
        PaymentResponse Authorise(string merchantReference, Money amount, PaymentCard card);

        /// <summary>
        /// Attempt to capture and amount that was previously authorised.
        /// </summary>
        //TODO Capture params...
        PaymentResponse Capture();
    }
}