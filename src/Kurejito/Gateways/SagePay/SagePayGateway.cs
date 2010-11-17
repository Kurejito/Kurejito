using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kurejito.Payments;

namespace Kurejito.Gateways.SagePay {
	public class SagePayGateway : IPurchaseGateway {

		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, BankCard card) {
			throw new NotImplementedException();
		}
	}
}
