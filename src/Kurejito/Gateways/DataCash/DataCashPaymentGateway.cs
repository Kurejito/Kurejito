﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Kurejito.Transport;
using Kurejito.Payments;

namespace Kurejito.Gateways.DataCash {
	public class DataCashPaymentGateway : IPurchaseGateway {

		private enum Method {
			auth,
			pre,
			refund,
			erp
		}

		private string client;
		private string password;
		private IHttpPostTransport http;

		/// <summary>Construct a new instance of the <see cref="DataCashPaymentGateway"/>.</summary>
		/// <param name="client">The client code for your payment gateway, as supplied by DataCash.</param>
		/// <param name="password">The password for your payment gateway, as supplised by DataCash.</param>
		public DataCashPaymentGateway(IHttpPostTransport http, string client, string password) {
			this.http = http;
			this.client = client;
			this.password = password;
		}

		private XElement MakeAuthenticationElement() {
			return (new XElement("Authentication", new XElement("client", client), new XElement("password", password)));
		}

		private XElement MakeCardTxnElement(PaymentCard card, Method method) {
			var cardElement = new XElement("Card",
				new XElement("pan", card.CardNumber),
				new XElement("expirydate", card.ExpiryDate.MM_YY)
			);
			if (card.HasStartDate) cardElement.Add(new XElement("startdate", card.StartDate.MM_YY));
			if (card.HasIssueNumber) cardElement.Add(new XElement("issuenumber", card.IssueNumber));

			var cardTxnElement = new XElement("CardTxn", 
				cardElement,
				new XElement("method", method)
			);
			return (cardTxnElement);
		}

		private XElement MakeTxnDetailsElement(string merchantreference, decimal amount, string currency) {
			return (new XElement("TxnDetails",
				new XElement("merchantreference", merchantreference),
				new XElement("amount", new XAttribute("currency", currency), amount.ToString("0.00"))
			));
		}

		private XElement MakeTransactionElement(string merchantreference, decimal amount, string currency, PaymentCard card, Method method) {
			return (new XElement("Transaction",
					MakeCardTxnElement(card, method),
					MakeTxnDetailsElement(merchantreference, amount, currency)
				));
		}

		public PaymentResponse Purchase(string merchantReference, decimal amount, string currency, PaymentCard card) {
			var xml = new XDocument(
				new XDeclaration("1.0", "UTF-8", null),
				new XElement("Request",
					MakeAuthenticationElement(),
					MakeTransactionElement(merchantReference, amount, currency, card, Method.auth)
				)
			);
			var response = http.Post(new Uri("https://testserver.datacash.com/Transaction"), xml.ToString(SaveOptions.DisableFormatting));
			return (new PaymentResponse() { Reason = response });
		}
	}
}
