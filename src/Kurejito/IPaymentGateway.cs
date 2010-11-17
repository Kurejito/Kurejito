using Kurejito.Gateway;
using Kurejito.Payment;

namespace Kurejito {
	public interface IPaymentGateway {
		PaymentResponse Purchase(Money money, CreditCard creditCard);
	}
}	
