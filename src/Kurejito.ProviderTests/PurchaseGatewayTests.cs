using System;
using Xunit;
using Kurejito.Payments;
using System.Diagnostics;
using Kurejito.Tests.Encoding;

namespace Kurejito.ProviderTests {
	public abstract class PurchaseGatewayTests {

		protected const string EXPIRY_DATE = "0515";

		protected virtual string GenerateMerchantReference() {
			var guid = Guid.NewGuid();
			return (Base32Url.ToBase32String(guid.ToByteArray()));
		}


		/// <summary>Implement this method to return an instance of the gateway that's being tested</summary>
		protected abstract IPurchaseGateway CreateGateway();

		/// <summary>Return a set of <see cref="PaymentCard" /> values that will return the 
		/// specified response type when submitted to this provider's test gateway.</summary>
		protected abstract PaymentCard GetMagicCard(PaymentStatus status);

		protected virtual PaymentResponse SubmitPayment(PaymentStatus requiredStatus) {
			var gw = CreateGateway();
			var card = this.GetMagicCard(requiredStatus);
			var amount = 123.45m;
			var merchantReference = this.GenerateMerchantReference();
			return(gw.Purchase(merchantReference, amount, "GBP", card));
		}

		[Fact]
		public void Successful_Purchase_Returns_Correct_PaymentStatus() {
			var response = this.SubmitPayment(PaymentStatus.Ok);
			Debug.WriteLine(response.Reason);
			Assert.Equal(PaymentStatus.Ok, response.Status);
		}

		[Fact]
		public void Declined_Purchase_Returns_Correct_PaymentStatus() {
			var response = this.SubmitPayment(PaymentStatus.Declined);
			Debug.WriteLine(response.Reason);
			Assert.Equal(PaymentStatus.Declined, response.Status);
		}

		[Fact]
		public void Purchase_With_Invalid_Card_Number_Returns_Correct_PaymentStatus() {
			var response = this.SubmitPayment(PaymentStatus.Invalid);
			Debug.WriteLine(response.Reason);
			Assert.Equal(PaymentStatus.Invalid, response.Status);
		}

	}
}
