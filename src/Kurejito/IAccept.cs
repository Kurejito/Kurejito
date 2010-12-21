using Kurejito.Payments;

namespace Kurejito {
    /// <summary>
    /// Interface implemented by Gateways in order to explain what currency/card combinations they can accept.
    /// </summary>
    public interface IAccept {
        /// <summary>
        /// Determines if the Gateway can accept the <paramref name="currency"/> <paramref name="cardType"/> combinations in calls to <see cref="IPurchase"/> and <see cref="IAuthoriseAndCapture"/> calls.
        /// </summary>
        bool Accepts(Currency currency, CardType cardType);
    }
}