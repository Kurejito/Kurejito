using System;
namespace Kurejito.Payments {
	/// <summary>Represents a credit or debit card used for online payments</summary>
	public class PaymentCard {
		/// <summary>The card holder's name, as it appears on the payment card.</summary>
		public string CardHolder { get; set; }

		/// <summary>The PAN, or Personal Account Number - normally the long number across the front of the payment card.</summary>
		public string CardNumber { get; set; }

		/// <summary>The card start date, where available</summary>
		public CardDate StartDate { get; set; }

		///<summary>The card expiry date</summary>
		public CardDate ExpiryDate { get; set; }

		/// <summary>The card issue number, found on somee Maestro and Solo cards.</summary>
		public int? IssueNumber { get; set; }

		/// <summary>The card verification value, normally the extra three digits on the card signature strip.</summary>
		public string CV2 { get; set; }

		/// <summary>Initialize an empty instance of <see cref="PaymentCard"/></summary>
		public PaymentCard() { }

	    /// <summary>Initialize a new instance of <see cref="PaymentCard"/> based on the supplied values.</summary>
	    /// <param name="cardHolder">The full cardholder name, as it appears on the payment card.</param>
	    /// <param name="cardNumber">The PAN number - the long number across the front of the card</param>
	    /// <param name="expiryDate">The card expiry date</param>
	    /// <param name="cv2">The Card Verification Value or 'signature digits' from the payment card.</param>
	    /// <param name="cardType"></param>
	    public PaymentCard(string cardHolder, string cardNumber, CardDate expiryDate, string cv2, CardType cardType) {
			this.CardHolder = cardHolder;
			this.CardNumber = cardNumber;
			this.ExpiryDate = expiryDate;
			this.CV2 = cv2;
			this.CardType = cardType;

		}

		/// <summary>The type of payment card (Visa, Mastercard, etc).</summary>
		public CardType CardType { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has start date.
        /// </summary>
		public bool HasStartDate { get { return (this.StartDate != null && this.StartDate.HasValue); } }

        /// <summary>
        /// Gets a value indicating whether this instance has issue number.
        /// </summary>
		public bool HasIssueNumber { get { return (this.IssueNumber.HasValue); } }
	}
}