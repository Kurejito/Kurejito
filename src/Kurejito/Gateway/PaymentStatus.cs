using System;
using System.Collections.Generic;
using System.Text;

namespace Kurejito.Gateway {
	public enum PaymentStatus {
		/// <summary>The payment was taken successfully.</summary>
		OK,
		/// <summary>The payment was declined by the card's issuing bank</summary>
		Declined,
		/// <summary>The payment request was not valid - e.g. incorrect card number or card already expired.</summary>
		Invalid,
		///<summary>Something went wrong trying to process this payment</summary>
		Error
	}
}
