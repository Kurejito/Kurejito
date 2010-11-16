using Kurejito.Payments;

namespace Kurejito.Gateway {
	public interface IPaymentGateway {
		PaymentResponse Purchase(Money money, CreditCard creditCard);
	}
}	
