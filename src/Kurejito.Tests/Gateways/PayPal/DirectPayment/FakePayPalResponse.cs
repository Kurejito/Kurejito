using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Kurejito.Tests.Gateways.PayPal.DirectPayment {
    public class FakePayPalResponse {
        private readonly NameValueCollection requestPairs;

        private FakePayPalResponse(NameValueCollection requestPairs) {
            if (requestPairs == null) throw new ArgumentNullException("requestPairs");
            this.requestPairs = requestPairs;
        }

        public static FakePayPalResponse Success {
            get {
                var requestPairs = HttpUtility.ParseQueryString(@"TIMESTAMP=2010%2d12%2d12T16%3a12%3a00Z&CORRELATIONID=9809a922ec57b&ACK=Success&VERSION=56%2e0&BUILD=1603674&AMT=100%2e00&CURRENCYCODE=GBP&AVSCODE=X&CVV2MATCH=S&TRANSACTIONID=58840544LM668971C");
                return new FakePayPalResponse(requestPairs);
            }
        }

        public override string ToString()
        {
            var values = requestPairs.AllKeys.Select(key => String.Format("{0}={1}", key, HttpUtility.UrlEncode(requestPairs[(string) key])));
            return (String.Join("&", values.ToArray()));
        }
    }
}