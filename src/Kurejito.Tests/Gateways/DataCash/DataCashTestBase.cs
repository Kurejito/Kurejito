using System;
using Kurejito.Payments;
using Kurejito.Tests.Encoding;

namespace Kurejito.Tests.Gateways.DataCash {
	public abstract class DataCashTestBase {
		protected const string CLIENT_ID = "99002086";
		protected const string PASSWORD = "AAm9YtKK";

		protected const string GATEWAY_URI = "https://testserver.datacash.com/Transaction";

		protected static class TestData {
			public static string MerchantReference = Base32Url.ToBase32String(Guid.NewGuid().ToByteArray());
			public static Money Amount = new Money(123.45m, new Currency("GBP"));
			public static PaymentCard Card = new PaymentCard("I M LOADED", "1234123413241234", "1212", "123", CardType.Visa);
		}

		private const string XML_RESPONSE_FORMAT = @"<?xml version=""1.0"" encoding=""UTF-8""?>
  <Response>
    <CardTxn>
      <authcode>100000</authcode>
      <card_scheme>VISA</card_scheme>
    </CardTxn>
    <datacash_reference>{2}</datacash_reference>
    <merchantreference>ABCDEFGHIJKLMNOPQRSTUVWXYZ</merchantreference>
    <mode>TEST</mode>
    <reason>{1}</reason>
    <status>{0}</status>
    <time>1290901997</time>
  </Response>";

		protected string MakeXmlResponse(int status, string reason) {
			return (MakeXmlResponse(status, reason, "1234123412341234"));
		}

		protected string MakeXmlResponse(int status, string reason, string dataCashReference) {
			return (String.Format(XML_RESPONSE_FORMAT, status, reason, dataCashReference));
		}
	}
}