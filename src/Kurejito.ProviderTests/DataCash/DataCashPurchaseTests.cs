using System;
using Kurejito.Tests.Gateways;
using Kurejito.Gateways.DataCash;
using Kurejito.Transport;
using Kurejito.Payments;

namespace Kurejito.ProviderTests.DataCash {
	public class DataCashPurchaseTests : PurchaseGatewayTests {

		private const string CLIENT_ID = "99002086";
		private const string PASSWORD = "QAVgBZMQ5Jt9";
		private const string GATEWAY_URI = "https://testserver.datacash.com/Transaction";

		protected override IPurchaseGateway CreateGateway() {
			var http = new HttpTransport();
			return (new DataCashPaymentGateway(http, CLIENT_ID, PASSWORD, GATEWAY_URI));
		}

		protected override Payments.PaymentCard GetMagicCard(PaymentStatus status) {
			// See DataCash magic cards reference at https://testserver.datacash.com/software/download.cgi?show=magicnumbers
			switch(status) {
				case PaymentStatus.Undefined:
					break;
				case PaymentStatus.Ok:
					return (new PaymentCard("I M LOADED", "1000189853512019", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Declined:
					return (new PaymentCard("I M SKINT", "100063000000007", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Invalid:
					return (new PaymentCard("1234123412341234", "I M STUPID", EXPIRY_DATE, "123", CardType.Visa));
				case PaymentStatus.Referred:
					return (new PaymentCard("I M DODGY", "1000350000000122", EXPIRY_DATE, "123", CardType.MasterCard));
			}
			throw new ArgumentOutOfRangeException("status");
		}

		//protected override decimal GetMagicAmount(PaymentStatus status) {
		//    switch (status) {
		//        case PaymentStatus.Ok:
		//            return (1000.00m);
		//        case PaymentStatus.Declined:
		//            return (1000.02m);
		//        case PaymentStatus.Invalid:
		//            return (1000.04m);
		//        case PaymentStatus.Referred:
		//            return (1000.01m);
		//        default:
		//            throw (new ArgumentException("status", "DataCash doesn't have a magic amount that'll trigger that response type"));
		//    }
		//}
	}
}
