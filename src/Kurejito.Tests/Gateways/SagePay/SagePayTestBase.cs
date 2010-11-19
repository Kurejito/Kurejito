using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Kurejito.Transport;
using Kurejito.Gateways.SagePay.VspDirect;
using Kurejito.Payments;

namespace Kurejito.Tests.Gateways.SagePay {
	public abstract class SagePayTestBase {
		protected Mock<IHttpPostTransport> http;

		protected SagePayPaymentGateway gateway;
		protected PaymentCard card;

		protected const string VENDOR_NAME = "rockshop";
		protected const decimal VPS_PROTOCOL = 2.23m;

	}
}
