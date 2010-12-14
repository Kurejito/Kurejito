using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Kurejito.Tests.Gateways.PayPal.DirectPayment {
    public class FakePayPalResponse {
        private const string ERROR_RESPONSE = @"TIMESTAMP=2010%2d12%2d12T16%3a15%3a11Z&CORRELATIONID=a9adbf79d0949&ACK={{ACK}}&VERSION=56%2e0&BUILD=1620725&L_ERRORCODE0={{ERROR_CODE}}&L_SHORTMESSAGE0={{SHORT_MESSAGE}}&L_LONGMESSAGE0=This%20transaction%20cannot%20be%20processed%2e&L_SEVERITYCODE0=Error&L_ERRORPARAMID0=ProcessorResponse&L_ERRORPARAMVALUE0=0000&AMT=105%2e05&CURRENCYCODE=GBP&AVSCODE=X&CVV2MATCH=S";
        private const string SUCCESS_RESPONSE = @"TIMESTAMP=2010%2d12%2d12T16%3a12%3a00Z&CORRELATIONID=9809a922ec57b&ACK=Success&VERSION=56%2e0&BUILD=1603674&AMT=100%2e00&CURRENCYCODE=GBP&AVSCODE=X&CVV2MATCH=S&TRANSACTIONID=58840544LM668971C";

        private readonly NameValueCollection requestPairs;

        private FakePayPalResponse(NameValueCollection requestPairs) {
            if (requestPairs == null) throw new ArgumentNullException("requestPairs");
            this.requestPairs = requestPairs;
        }

        public static FakePayPalResponse Success {
            get {
                var requestPairs = HttpUtility.ParseQueryString(SUCCESS_RESPONSE);
                return new FakePayPalResponse(requestPairs);
            }
        }

        public static implicit operator string(FakePayPalResponse fakePayPalResponse) {
            return fakePayPalResponse.ToString();
        }

        public override string ToString() {
            var values = this.requestPairs.AllKeys.Select(key => String.Format("{0}={1}", key, HttpUtility.UrlEncode(this.requestPairs[key])));
            return (String.Join("&", values.ToArray()));
        }

        public static FakePayPalResponse AckFailure() {
            return CreateErrorResponseWithAck("Failure");
        }

        public static FakePayPalResponse AckPartialSuccess() {
            return CreateErrorResponseWithAck("PartialSuccess");
        }

        private static FakePayPalResponse CreateErrorResponseWithAck(string ackValue) {
            var nameValueCollection = HttpUtility.ParseQueryString(ERROR_RESPONSE);
            nameValueCollection["ACK"] = ackValue;
            return new FakePayPalResponse(nameValueCollection);
        }

        public FakePayPalResponse WithError(string shortMessage, string errorCode) {
            if (shortMessage == null) throw new ArgumentNullException("shortMessage");
            if (errorCode == null) throw new ArgumentNullException("errorCode");
            this.requestPairs["L_SHORTMESSAGE0"] = shortMessage;
            this.requestPairs["L_ERRORCODE0"] = errorCode;
            return this;
        }
    }
}